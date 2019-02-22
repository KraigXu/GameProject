using UnityEngine;

namespace Invector.vShooter
{
    using IK;
    using vCharacterController;
    [vClassHeader("SHOOTER/MELEE INPUT", iconName = "inputIcon")]
    public class vShooterMeleeInput : vMeleeCombatInput
    {
        #region Shooter Inputs

        [vEditorToolbar("Inputs")]
        [Header("Shooter Inputs")]
        public GenericInput aimInput = new GenericInput("Mouse1", false, "LT", true, "LT", false);
        public GenericInput shotInput = new GenericInput("Mouse0", false, "RT", true, "RT", false);
        public GenericInput secundaryShotInput = new GenericInput("Mouse2", false, "X", true, "X", false);
        public GenericInput reloadInput = new GenericInput("R", "LB", "LB");
        public GenericInput switchCameraSideInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");
        public GenericInput scopeViewInput = new GenericInput("Z", "RB", "RB");

        #endregion

        #region Shooter Variables       

        internal vShooterManager shooterManager;
        internal bool blockAim;
        internal bool isAiming;
        internal bool canEquip;
        internal bool isReloading;
        internal bool isEquipping;
        internal Transform leftHand, rightHand, rightLowerArm, leftLowerArm, rightUpperArm, leftUpperArm;
        internal Vector3 aimPosition;
        internal float aimTimming;

        protected int onlyArmsLayer;
        protected int shootCountA;
        protected int shootCountB;
        protected bool allowAttack;
        protected bool aimConditions;
        protected bool isUsingScopeView;
        protected bool isCameraRightSwitched;
        protected float onlyArmsLayerWeight;
        protected float lIKWeight;
        protected float armAlignmentWeight;
        protected float aimWeight;

        protected float lastAimDistance;
        protected Quaternion handRotation, upperArmRotation;
        protected vIKSolver leftIK, rightIK;
        protected vHeadTrack headTrack;
        private vControlAimCanvas _controlAimCanvas;
        private GameObject aimAngleReference;

        private Vector3 lastUpperArmRotation;
        private Vector3 lastLowerArmRotation;
        private Vector3 lastHandRotation;
        private Vector3 lastIKHandPosition;
        private Vector3 lastIKHandRotation;

        private Vector3 currentUpperArmOffset;
        private Vector3 currentLowerArmOffset;
        private Vector3 currentHandOffset;
        private Vector3 ikRotationOffset;
        private Vector3 ikPositionOffset;
        private Quaternion upperArmRotationAlignment, handRotationAlignment;

        public vControlAimCanvas controlAimCanvas
        {
            get
            {
                if (!_controlAimCanvas)
                {
                    _controlAimCanvas = FindObjectOfType<vControlAimCanvas>();
                    if (_controlAimCanvas)
                        _controlAimCanvas.Init(cc);
                }

                return _controlAimCanvas;
            }
        }

        internal bool lockShooterInput;

        #endregion

        protected override void Start()
        {
            shooterManager = GetComponent<vShooterManager>();

            base.Start();

            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);

            onlyArmsLayer = animator.GetLayerIndex("OnlyArms");
            aimAngleReference = new GameObject("aimAngleReference");
            aimAngleReference.tag = ("Ignore Ragdoll");
            aimAngleReference.transform.rotation = transform.rotation;
            var chest = animator.GetBoneTransform(HumanBodyBones.Head);
            aimAngleReference.transform.SetParent(chest);
            aimAngleReference.transform.localPosition = Vector3.zero;

            headTrack = GetComponent<vHeadTrack>();

            if (!controlAimCanvas)
                Debug.LogWarning("Missing the AimCanvas, drag and drop the prefab to this scene in order to Aim", gameObject);
        }

        protected override void LateUpdate()
        {
            if ((!updateIK && animator.updateMode == AnimatorUpdateMode.AnimatePhysics)) return;
            base.LateUpdate();
            UpdateAimBehaviour();
        }

        #region Shooter Inputs    

        /// <summary>
        /// Lock all shooter inputs
        /// </summary>
        /// <param name="value">lock or unlock</param>
        public virtual void SetLockShooterInput(bool value)
        {
            lockShooterInput = value;

            if (value)
            {
                cc.isStrafing = false;
                isBlocking = false;
                isAiming = false;
                aimTimming = 0f;
                if (controlAimCanvas)
                {
                    controlAimCanvas.SetActiveAim(false);
                    controlAimCanvas.SetActiveScopeCamera(false);
                }
            }
        }

        protected override void InputHandle()
        {
            if (cc == null || lockInput || cc.isDead)
                return;

            #region BasicInput

            if (!isAttacking)
            {
                if (!cc.lockMovement && !cc.ragdolled)
                {
                    MoveCharacter();
                    SprintInput();
                    CrouchInput();
                    StrafeInput();
                    JumpInput();
                    RollInput();
                }
            }
            else
                cc.input = Vector2.zero;

            #endregion

            #region MeleeInput

            if (MeleeAttackConditions && !isAiming && !isReloading && !lockMeleeInput && (shooterManager.CurrentWeapon == null || (CurrentActiveWeapon == null && !shooterManager.hipfireShot)))
            {
                MeleeWeakAttackInput();
                MeleeStrongAttackInput();
                BlockingInput();
            }
            else
                isBlocking = false;

            #endregion

            #region ShooterInput

            if (lockShooterInput)
            {
                isAiming = false;
            }
            else
            {
                if (shooterManager == null || CurrentActiveWeapon == null || isEquipping)
                {
                    if (isAiming)
                    {
                        isAiming = false;
                        if (cc.isStrafing) cc.Strafe();
                        if (controlAimCanvas != null)
                        {
                            controlAimCanvas.SetActiveAim(false);
                            controlAimCanvas.SetActiveScopeCamera(false);
                        }
                        if (shooterManager && shooterManager.CurrentWeapon && shooterManager.CurrentWeapon.chargeWeapon && shooterManager.CurrentWeapon.powerCharge != 0) CurrentActiveWeapon.powerCharge = 0;
                        if (shooterManager && shooterManager.CurrentWeapon && shooterManager.CurrentWeapon.secundaryWeapon != null && shooterManager.CurrentWeapon.secundaryWeapon.chargeWeapon && shooterManager.CurrentWeapon.secundaryWeapon.powerCharge != 0) shooterManager.CurrentWeapon.secundaryWeapon.powerCharge = 0;
                        shootCountA = 0;
                        shootCountB = 0;
                    }
                }
                else
                {
                    AimInput();
                    ShotInput();
                    ReloadInput();
                    SwitchCameraSideInput();
                    ScopeViewInput();
                }
            }
            onUpdateInput.Invoke(this);

            #endregion
        }

        public override bool lockInventory
        {
            get
            {
                return base.lockInventory || isReloading;
            }
        }

        /// <summary>
        /// Current active weapon (if weapon gameobject is disabled this return null)
        /// </summary>
        public virtual vShooterWeapon CurrentActiveWeapon
        {
            get
            {
                return shooterManager.CurrentWeapon && shooterManager.IsCurrentWeaponActive() ? shooterManager.CurrentWeapon : null;
            }
        }

        /// <summary>
        /// Set Always Aiming
        /// </summary>
        /// <param name="value">value to set aiming</param>
        public virtual void AlwaysAim(bool value)
        {
            shooterManager.alwaysAiming = value;
        }

        /// <summary>
        /// Control Aim Input
        /// </summary>
        public virtual void AimInput()
        {
            if (!shooterManager || isAttacking)
            {
                isAiming = false;
                if (controlAimCanvas)
                {
                    controlAimCanvas.SetActiveAim(false);
                    controlAimCanvas.SetActiveScopeCamera(false);
                }
                if (cc.isStrafing) cc.Strafe();
                return;
            }

            if (cc.locomotionType == vThirdPersonMotor.LocomotionType.OnlyFree)
            {
                Debug.LogWarning("Shooter behaviour needs to be OnlyStrafe or Free with Strafe. \n Please change the Locomotion Type.");
                return;
            }

            if (shooterManager.hipfireShot)
            {
                if (aimTimming > 0)
                    aimTimming -= Time.deltaTime;
            }

            if (!shooterManager || !CurrentActiveWeapon)
            {
                if (controlAimCanvas)
                {
                    controlAimCanvas.SetActiveAim(false);
                    controlAimCanvas.SetActiveScopeCamera(false);
                }
                isAiming = false;
                if (cc.isStrafing) cc.Strafe();
                return;
            }

            if (!cc.isRolling)
                isAiming = !isReloading && (aimInput.GetButton() || (shooterManager.alwaysAiming)) && !cc.ragdolled && !cc.actions && !cc.customAction || (cc.actions && cc.isJumping);

            if (headTrack)
                headTrack.awaysFollowCamera = isAiming;
            var _aiming = (isAiming || aimTimming > 0);
            if (cc.locomotionType == vThirdPersonMotor.LocomotionType.FreeWithStrafe)
            {
                if (_aiming && !cc.isStrafing)
                {
                    cc.Strafe();
                }
                else if (!_aiming && cc.isStrafing)
                {
                    cc.Strafe();
                }
            }
            if (_aiming && shooterManager.onlyWalkWhenAiming && cc.isSprinting) cc.isSprinting = false;
            if (controlAimCanvas)
            {
                if (_aiming && !controlAimCanvas.isAimActive)
                    controlAimCanvas.SetActiveAim(true);
                if (!_aiming && controlAimCanvas.isAimActive)
                    controlAimCanvas.SetActiveAim(false);
            }
            if (shooterManager.rWeapon)
            {
                shooterManager.rWeapon.SetActiveAim(_aiming && aimConditions);
                shooterManager.rWeapon.SetActiveScope(_aiming && isUsingScopeView);
            }
            else if (shooterManager.lWeapon)
            {
                shooterManager.lWeapon.SetActiveAim(_aiming && aimConditions);
                shooterManager.lWeapon.SetActiveScope(_aiming && isUsingScopeView);
            }
        }

        /// <summary>
        /// Control shot inputs (primary and secundary weapons)
        /// </summary>
        public virtual void ShotInput()
        {
            if (!shooterManager || CurrentActiveWeapon == null || cc.isDead)
            {
                if (shooterManager && shooterManager.CurrentWeapon.chargeWeapon && shooterManager.CurrentWeapon.powerCharge != 0) CurrentActiveWeapon.powerCharge = 0;
                if (shooterManager && shooterManager.CurrentWeapon.secundaryWeapon != null && shooterManager.CurrentWeapon.secundaryWeapon.chargeWeapon && shooterManager.CurrentWeapon.secundaryWeapon.powerCharge != 0) shooterManager.CurrentWeapon.secundaryWeapon.powerCharge = 0;
                shootCountA = 0;
                shootCountB = 0;
                return;
            }

            if ((isAiming && !shooterManager.hipfireShot || shooterManager.hipfireShot) && !shooterManager.isShooting && aimConditions && !isReloading && !isAttacking)
            {
                if (CurrentActiveWeapon || (shooterManager.CurrentWeapon && shooterManager.hipfireShot))
                {
                    HandleShotCount(shooterManager.CurrentWeapon, false, shotInput.GetButton());
                }
                if ((CurrentActiveWeapon || (shooterManager.CurrentWeapon && shooterManager.hipfireShot)) && shooterManager.CurrentWeapon.secundaryWeapon)
                {
                    HandleShotCount(shooterManager.CurrentWeapon.secundaryWeapon, true, secundaryShotInput.GetButton());
                }
            }
            else if (!isAiming)
            {
                if (shooterManager.CurrentWeapon.chargeWeapon && shooterManager.CurrentWeapon.powerCharge != 0) CurrentActiveWeapon.powerCharge = 0;
                if (shooterManager.CurrentWeapon.secundaryWeapon != null && shooterManager.CurrentWeapon.secundaryWeapon.chargeWeapon && shooterManager.CurrentWeapon.secundaryWeapon.powerCharge != 0) shooterManager.CurrentWeapon.secundaryWeapon.powerCharge = 0;
                shootCountA = 0;
                shootCountB = 0;
            }
            shooterManager.UpdateShotTime();
        }

        /// <summary>
        /// Control Shot count
        /// </summary>
        /// <param name="weapon">target weapon</param>
        /// <param name="secundaryShot">target weapon is secundary?</param>
        /// <param name="weaponInput">check input</param>
        public virtual void HandleShotCount(vShooterWeapon weapon, bool secundaryShot = false, bool weaponInput = true)
        {
            if (weapon.chargeWeapon)
            {
                if (shooterManager.WeaponHasAmmo(secundaryShot) && weapon.powerCharge < 1 && weaponInput)
                {
                    if (shooterManager.hipfireShot) aimTimming = shooterManager.hipfireAimTime;
                    weapon.powerCharge += Time.deltaTime * weapon.chargeSpeed;
                }
                else if ((weapon.powerCharge >= 1 && weapon.autoShotOnFinishCharge && weaponInput) || (!weaponInput && (isAiming || (shooterManager.hipfireShot && aimTimming > 0)) && weapon.powerCharge > 0))
                {
                    if (shooterManager.hipfireShot) aimTimming = shooterManager.hipfireAimTime;
                    if (secundaryShot)
                        shootCountB++;
                    else
                        shootCountA++;
                    weapon.powerCharge = 0;
                }
                animator.SetFloat("PowerCharger", weapon.powerCharge);
            }
            else if (weapon.automaticWeapon && weaponInput)
            {
                if (shooterManager.hipfireShot) aimTimming = shooterManager.hipfireAimTime;
                if (secundaryShot)
                    shootCountB++;
                else
                    shootCountA++;
            }
            else if (weaponInput)
            {
                if (allowAttack == false)
                {
                    if (shooterManager.hipfireShot) aimTimming = shooterManager.hipfireAimTime;
                    if (secundaryShot)
                        shootCountB++;
                    else
                        shootCountA++;
                    allowAttack = true;
                }
            }
            else allowAttack = false;
        }

        /// <summary>
        /// Do Shots by shotcount (primary and secundary weapons) after Ik behaviour updated
        /// </summary>
        public virtual void DoShots()
        {
            if (shootCountA > 0)
            {
                if (cc.IsAnimatorTag("Upperbody Pose"))
                {
                    shootCountA--;
                    shooterManager.Shoot(aimPosition, !isAiming, false);
                }
            }
            if (shootCountB > 0)
            {
                if (cc.IsAnimatorTag("Upperbody Pose"))
                {
                    shootCountB--;
                    shooterManager.Shoot(aimPosition, !isAiming, true);
                }
            }
        }

        /// <summary>
        /// Add Shot count to primary weapon
        /// </summary>
        /// <param name="inputValue">Press or Unpress.if current weapon is a charger weapon you want to call this method using false inputvalue afeter charger is started or finished</param>
        public virtual void ShotPrimary(bool inputValue = true)
        {
            if ((isAiming && !shooterManager.hipfireShot || shooterManager.hipfireShot) && !shooterManager.isShooting && aimConditions && !isReloading && !isAttacking)
            {
                if (CurrentActiveWeapon) HandleShotCount(CurrentActiveWeapon, false, true);
                if (CurrentActiveWeapon && CurrentActiveWeapon.secundaryWeapon)
                {
                    HandleShotCount(CurrentActiveWeapon.secundaryWeapon, true, (CurrentActiveWeapon.secundaryWeapon.automaticWeapon || CurrentActiveWeapon.secundaryWeapon.chargeWeapon) ? secundaryShotInput.GetButton() : secundaryShotInput.GetButtonDown());
                }
            }
        }

        /// <summary>
        /// Add Shot count to secundary weapon
        /// </summary>
        /// <param name="inputValue">Press or Unpress.If current weapon is a charger weapon you want to call this method using false inputvalue afeter charger is started or finished</param>
        public virtual void ShotSecundary(bool inputValue = true)
        {
            if ((isAiming && !shooterManager.hipfireShot || shooterManager.hipfireShot) && !shooterManager.isShooting && aimConditions && !isReloading && !isAttacking)
            {

                if (CurrentActiveWeapon && CurrentActiveWeapon.secundaryWeapon)
                    HandleShotCount(CurrentActiveWeapon.secundaryWeapon, true, true);
            }
        }

        /// <summary>
        /// Reaload current weapon
        /// </summary>
        public virtual void ReloadInput()
        {
            if (!shooterManager || CurrentActiveWeapon == null) return;
            if (reloadInput.GetButtonDown() && !cc.actions && !cc.ragdolled)
            {
                aimTimming = 0f;
                shooterManager.ReloadWeapon();
            }
        }

        /// <summary>
        /// Control Switch Camera side Input
        /// </summary>
        public virtual void SwitchCameraSideInput()
        {
            if (tpCamera == null) return;
            if (switchCameraSideInput.GetButtonDown())
            {
                SwitchCameraSide();
            }
        }

        /// <summary>
        /// Change side view of the <seealso cref="Invector.vCamera.vThirdPersonCamera"/>
        /// </summary>
        public virtual void SwitchCameraSide()
        {
            if (tpCamera == null) return;
            isCameraRightSwitched = !isCameraRightSwitched;
            tpCamera.SwitchRight(isCameraRightSwitched);
        }

        /// <summary>
        /// Control Scope view input
        /// </summary>
        public virtual void ScopeViewInput()
        {
            if (!shooterManager || CurrentActiveWeapon == null) return;

            if (isAiming && aimConditions && (scopeViewInput.GetButtonDown() || CurrentActiveWeapon.onlyUseScopeUIView))
            {
                if (controlAimCanvas && CurrentActiveWeapon.scopeTarget)
                {
                    if (!isUsingScopeView && CurrentActiveWeapon.onlyUseScopeUIView) EnableScorpeView();
                    else if (isUsingScopeView && !CurrentActiveWeapon.onlyUseScopeUIView) DisableScopeView();
                    else if (!isUsingScopeView) EnableScorpeView();
                }
            }
            else if (isUsingScopeView && (controlAimCanvas && !isAiming || controlAimCanvas && !aimConditions || cc.isRolling))
            {
                DisableScopeView();
            }
        }

        /// <summary>
        /// Enable scope view (just if is aiming)
        /// </summary>
        public virtual void EnableScorpeView()
        {
            if (!isAiming) return;
            isUsingScopeView = true;
            controlAimCanvas.SetActiveScopeCamera(true, CurrentActiveWeapon.useUI);
        }

        /// <summary>
        /// Disable scope view
        /// </summary>
        public virtual void DisableScopeView()
        {
            isUsingScopeView = false;
            controlAimCanvas.SetActiveScopeCamera(false);
        }

        public override void BlockingInput()
        {
            if (shooterManager == null || CurrentActiveWeapon == null)
                base.BlockingInput();
        }

        public override void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !cc.actions && !cc.lockMovement && rotateToCameraWhileStrafe)
            {
                // smooth align character with aim position
                if (tpCamera != null && tpCamera.lockTarget)
                {
                    cc.RotateToTarget(tpCamera.lockTarget);
                }
                // rotate the camera around the character and align with when the char move
                else if (cc.input != Vector2.zero || (isAiming || aimTimming > 0))
                {
                    cc.RotateWithAnotherTransform(cameraTransform);
                }
            }
        }

        #endregion

        #region Update Animations

        protected override void UpdateMeleeAnimations()
        {
            // disable the onlyarms layer and run the melee methods if the character is not using any shooter weapon
            if (!animator) return;

            // update MeleeManager Animator Properties
            if ((shooterManager == null || !CurrentActiveWeapon) && meleeManager)
            {
                base.UpdateMeleeAnimations();
                // set the uppbody id (armsonly layer)
                animator.SetFloat("UpperBody_ID", 0, .2f, Time.deltaTime);
                // turn on the onlyarms layer to aim 
                onlyArmsLayerWeight = Mathf.Lerp(onlyArmsLayerWeight, 0, 6f * Time.deltaTime);
                animator.SetLayerWeight(onlyArmsLayer, onlyArmsLayerWeight);
                // reset aiming parameter
                animator.SetBool("IsAiming", false);
                isReloading = false;
            }
            // update ShooterManager Animator Properties
            else if (shooterManager && CurrentActiveWeapon)
                UpdateShooterAnimations();
            // reset Animator Properties
            else
            {
                // set the move set id (base layer) 
                animator.SetFloat("MoveSet_ID", 0, .1f, Time.deltaTime);
                // set the uppbody id (armsonly layer)
                animator.SetFloat("UpperBody_ID", 0, .2f, Time.deltaTime);
                // set if the character can aim or not (upperbody layer)
                animator.SetBool("CanAim", false);
                // character is aiming
                animator.SetBool("IsAiming", false);
                // turn on the onlyarms layer to aim 
                onlyArmsLayerWeight = Mathf.Lerp(onlyArmsLayerWeight, 0, 6f * Time.deltaTime);
                animator.SetLayerWeight(onlyArmsLayer, onlyArmsLayerWeight);
            }
        }

        protected virtual void UpdateShooterAnimations()
        {
            if (shooterManager == null) return;

            if ((!isAiming && aimTimming <= 0) && meleeManager)
            {
                // set attack id from the melee weapon (trigger fullbody atk animations)
                animator.SetInteger("AttackID", meleeManager.GetAttackID());
            }
            else
            {
                // set attack id from the shooter weapon (trigger shot layer animations)
                animator.SetFloat("Shot_ID", shooterManager.GetShotID());
            }
            // turn on the onlyarms layer to aim 
            onlyArmsLayerWeight = Mathf.Lerp(onlyArmsLayerWeight, (CurrentActiveWeapon) ? 1f : 0f, 6f * Time.deltaTime);
            animator.SetLayerWeight(onlyArmsLayer, onlyArmsLayerWeight);

            if (CurrentActiveWeapon != null && !shooterManager.useDefaultMovesetWhenNotAiming || (isAiming || aimTimming > 0))
            {
                // set the move set id (base layer) 
                animator.SetFloat("MoveSet_ID", shooterManager.GetMoveSetID(), .1f, Time.deltaTime);
            }
            else if (shooterManager.useDefaultMovesetWhenNotAiming)
            {
                // set the move set id (base layer) 
                animator.SetFloat("MoveSet_ID", 0, .1f, Time.deltaTime);
            }
            // set the isBlocking false while using shooter weapons
            animator.SetBool("IsBlocking", false);
            // set the uppbody id (armsonly layer)
            animator.SetFloat("UpperBody_ID", shooterManager.GetUpperBodyID(), .2f, Time.deltaTime);
            // set if the character can aim or not (upperbody layer)
            animator.SetBool("CanAim", aimConditions);
            // character is aiming
            animator.SetBool("IsAiming", (isAiming || aimTimming > 0) && !isAttacking);
            // find states with the Reload tag
            isReloading = cc.IsAnimatorTag("IsReloading") || shooterManager.isReloadingWeapon;
            // find states with the IsEquipping tag
            isEquipping = cc.IsAnimatorTag("IsEquipping");
        }

        protected override void UpdateCameraStates()
        {
            // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData

            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vCamera.vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }

            if (changeCameraState)
                tpCamera.ChangeState(customCameraState, customlookAtPoint, true);
            else if (cc.isCrouching)
                tpCamera.ChangeState("Crouch", true);
            else if (cc.isStrafing && !isAiming)
                tpCamera.ChangeState("Strafing", true);
            else if (isAiming && CurrentActiveWeapon)
            {
                if (string.IsNullOrEmpty(CurrentActiveWeapon.customAimCameraState))
                    tpCamera.ChangeState("Aiming", true);
                else
                    tpCamera.ChangeState(CurrentActiveWeapon.customAimCameraState, true);
            }
            else
                tpCamera.ChangeState("Default", true);
        }

        #endregion

        #region Update Aim

        protected virtual void UpdateAimPosition()
        {
            if (!shooterManager) return;

            if (CurrentActiveWeapon == null) return;

            var camT = isUsingScopeView && controlAimCanvas && controlAimCanvas.scopeCamera ? //Check if is using canvas scope view
                    CurrentActiveWeapon.zoomScopeCamera ? /* if true, check if weapon has a zoomScopeCamera, 
                if true...*/
                    CurrentActiveWeapon.zoomScopeCamera.transform : controlAimCanvas.scopeCamera.transform :
                    /*else*/Camera.main.transform;

            var origin1 = camT.position;
            if (!(controlAimCanvas && controlAimCanvas.isScopeCameraActive && controlAimCanvas.scopeCamera))
                origin1 = camT.position;

            var vOrigin = origin1;
            vOrigin += controlAimCanvas && controlAimCanvas.isScopeCameraActive && controlAimCanvas.scopeCamera ? camT.forward : Vector3.zero;
            aimPosition = camT.position + camT.forward * 100f;
            //aimAngleReference.transform.eulerAngles = new Vector3(aimAngleReference.transform.eulerAngles.x, transform.eulerAngles.y, aimAngleReference.transform.eulerAngles.z);
            if (!isUsingScopeView) lastAimDistance = 100f;

            if (shooterManager.raycastAimTarget && CurrentActiveWeapon.raycastAimTarget)
            {
                RaycastHit hit;
                Ray ray = new Ray(vOrigin, camT.forward);

                if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, shooterManager.damageLayer))
                {
                    if (hit.collider.transform.IsChildOf(transform))
                    {
                        var collider = hit.collider;
                        var hits = Physics.RaycastAll(ray, Camera.main.farClipPlane, shooterManager.damageLayer);
                        var dist = Camera.main.farClipPlane;
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].distance < dist && hits[i].collider.gameObject != collider.gameObject && !hits[i].collider.transform.IsChildOf(transform))
                            {
                                dist = hits[i].distance;
                                hit = hits[i];
                            }
                        }
                    }

                    if (hit.collider)
                    {
                        if (!isUsingScopeView)
                            lastAimDistance = Vector3.Distance(camT.position, hit.point);
                        aimPosition = hit.point;
                    }
                }
                if (shooterManager.showCheckAimGizmos)
                {
                    Debug.DrawLine(ray.origin, aimPosition);
                }
            }
            if (isAiming)
                shooterManager.CameraSway();
        }

        #endregion

        #region IK behaviour

        void OnDrawGizmos()
        {
            if (!shooterManager || !shooterManager.showCheckAimGizmos) return;
            var weaponSide = isCameraRightSwitched ? -1 : 1;
            var _ray = new Ray(aimAngleReference.transform.position + transform.up * shooterManager.blockAimOffsetY + transform.right * shooterManager.blockAimOffsetX * weaponSide, Camera.main.transform.forward);
            Gizmos.DrawRay(_ray.origin, _ray.direction * shooterManager.minDistanceToAim);
            var color = Gizmos.color;
            color = aimConditions ? Color.green : Color.red;
            color.a = 1f;
            Gizmos.color = color;
            Gizmos.DrawSphere(_ray.GetPoint(shooterManager.minDistanceToAim), shooterManager.checkAimRadius);
            Gizmos.DrawSphere(aimPosition, shooterManager.checkAimRadius);
        }

        protected virtual void UpdateAimBehaviour()
        {

            UpdateAimPosition();
            UpdateHeadTrack();
            if (shooterManager && CurrentActiveWeapon)
            {
                RotateAimArm(shooterManager.IsLeftWeapon);
                RotateAimHand(shooterManager.IsLeftWeapon);
                UpdateArmsIK(shooterManager.IsLeftWeapon);
            }
            if (isUsingScopeView && controlAimCanvas && controlAimCanvas.scopeCamera) UpdateAimPosition();
            CheckAimConditions();
            UpdateAimHud();
            DoShots();
        }

        //protected virtual void ApplyArmOffset(bool isUsingLeftHand = false)
        //{
        //    if (ikOffset != null)
        //    {
        //        var upperArm = isUsingLeftHand ? leftUpperArm : rightUpperArm;
        //        var lowerArm = isUsingLeftHand ? leftLowerArm : rightLowerArm;
        //        var hand = isUsingLeftHand ? leftHand : rightHand;

        //        if (upperArm)
        //        {                    
        //            if (animator.isActiveAndEnabled) lastUpperArmRotation = upperArm.localEulerAngles;
        //            currentUpperArmOffset.x = Mathf.Lerp(currentUpperArmOffset.x, ikOffset.upperarmRotationOffset.x, 5f * Time.deltaTime);
        //            currentUpperArmOffset.y = Mathf.Lerp(currentUpperArmOffset.y, ikOffset.upperarmRotationOffset.y, 5f * Time.deltaTime);
        //            currentUpperArmOffset.z = Mathf.Lerp(currentUpperArmOffset.z, ikOffset.upperarmRotationOffset.z, 5f * Time.deltaTime);
        //            upperArm.localEulerAngles = lastUpperArmRotation + currentUpperArmOffset;
        //        }
        //        if (lowerArm)
        //        {
        //            if (animator.isActiveAndEnabled) lastLowerArmRotation = lowerArm.localEulerAngles;
        //            currentLowerArmOffset.x = Mathf.Lerp(currentLowerArmOffset.x, ikOffset.forearmRotationOffset.x, 5f * Time.deltaTime);
        //            currentLowerArmOffset.y = Mathf.Lerp(currentLowerArmOffset.y, ikOffset.forearmRotationOffset.y, 5f * Time.deltaTime);
        //            currentLowerArmOffset.z = Mathf.Lerp(currentLowerArmOffset.z, ikOffset.forearmRotationOffset.z, 5f * Time.deltaTime);
        //            lowerArm.localEulerAngles = lastLowerArmRotation + currentLowerArmOffset;
        //        }
        //        if (hand)
        //        {
        //            if (animator.isActiveAndEnabled) lastHandRotation = hand.localEulerAngles;
        //            currentHandOffset.x = Mathf.Lerp(currentHandOffset.x, ikOffset.handRotationOffset.x, 5f * Time.deltaTime);
        //            currentHandOffset.y = Mathf.Lerp(currentHandOffset.y, ikOffset.handRotationOffset.y, 5f * Time.deltaTime);
        //            currentHandOffset.z = Mathf.Lerp(currentHandOffset.z, ikOffset.handRotationOffset.z, 5f * Time.deltaTime);
        //            hand.localEulerAngles = lastHandRotation + currentHandOffset;
        //        }
        //    }
        //}

        protected virtual void UpdateArmsIK(bool isUsingLeftHand = false)
        {
            if (!shooterManager || !CurrentActiveWeapon || !shooterManager.useLeftIK) return;
            if (animator.GetCurrentAnimatorStateInfo(6).IsName("Shot Fire") && CurrentActiveWeapon.disableIkOnShot) { lIKWeight = 0; return; }

            bool useIkConditions = false;
            var animatorInput = cc.input.magnitude;
            if (!isAiming && !isAttacking)
            {
                if (animatorInput < 1f)
                    useIkConditions = CurrentActiveWeapon.useIkOnIdle;
                else if (cc.isStrafing)
                    useIkConditions = CurrentActiveWeapon.useIkOnStrafe;
                else
                    useIkConditions = CurrentActiveWeapon.useIkOnFree;
            }
            else if (isAiming && !isAttacking) useIkConditions = CurrentActiveWeapon.useIKOnAiming;
            else if (isAttacking) useIkConditions = CurrentActiveWeapon.useIkAttacking;

            // create left arm ik solver if equal null
            if (leftIK == null) leftIK = new vIKSolver(animator, AvatarIKGoal.LeftHand);
            if (rightIK == null) rightIK = new vIKSolver(animator, AvatarIKGoal.RightHand);
            vIKSolver targetIK = null;

            if (isUsingLeftHand)
                targetIK = rightIK;
            else
                targetIK = leftIK;

            if (targetIK != null)
            {
                if (isUsingLeftHand)
                {
                    ikRotationOffset = shooterManager.ikRotationOffsetR;
                    ikPositionOffset = shooterManager.ikPositionOffsetR;
                }
                else
                {
                    ikRotationOffset = shooterManager.ikRotationOffsetL;
                    ikPositionOffset = shooterManager.ikPositionOffsetL;
                }
                // control weight of ik
                if (CurrentActiveWeapon && CurrentActiveWeapon.handIKTarget && Time.timeScale > 0 && !isReloading && !cc.actions && !cc.customAction && (!animator.IsInTransition(4) || isAiming) && !isEquipping && (cc.isGrounded || (isAiming || aimTimming > 0f)) && !cc.lockMovement && useIkConditions)
                    lIKWeight = Mathf.Lerp(lIKWeight, 1, 10f * Time.deltaTime);
                else
                    lIKWeight = Mathf.Lerp(lIKWeight, 0, 25f * Time.deltaTime);

                if (lIKWeight <= 0) return;
                // update IK
                targetIK.SetIKWeight(lIKWeight);
                if (shooterManager && CurrentActiveWeapon && CurrentActiveWeapon.handIKTarget)
                {
                    var _offset = (CurrentActiveWeapon.handIKTarget.forward * ikPositionOffset.z) + (CurrentActiveWeapon.handIKTarget.right * ikPositionOffset.x) + (CurrentActiveWeapon.handIKTarget.up * ikPositionOffset.y);
                    targetIK.SetIKPosition(CurrentActiveWeapon.handIKTarget.position + _offset);
                    var _rotation = Quaternion.Euler(ikRotationOffset);
                    targetIK.SetIKRotation(CurrentActiveWeapon.handIKTarget.rotation * _rotation);
                }
            }
        }

        protected virtual void RotateAimArm(bool isUsingLeftHand = false)
        {
            if (!shooterManager) return;

            armAlignmentWeight = (isAiming || aimTimming > 0) && aimConditions ? Mathf.Lerp(armAlignmentWeight, 1f, 1f * (.001f + Time.deltaTime)) : 0;
            if (CurrentActiveWeapon && armAlignmentWeight > 0.1f && CurrentActiveWeapon.alignRightUpperArmToAim)
            {
                var aimPoint = targetArmAlignmentPosition;
                Vector3 v = aimPoint - CurrentActiveWeapon.aimReference.position;
                var orientation = CurrentActiveWeapon.aimReference.forward;

                var upperArm = isUsingLeftHand ? leftUpperArm : rightUpperArm;
                var rot = Quaternion.FromToRotation(upperArm.InverseTransformDirection(orientation), upperArm.InverseTransformDirection(v));

                if (!shooterManager.isShooting && (!float.IsNaN(rot.x) && !float.IsNaN(rot.y) && !float.IsNaN(rot.z)))
                    upperArmRotationAlignment = rot;

                var angle = Vector3.Angle(aimPosition - aimAngleReference.transform.position, aimAngleReference.transform.forward);

                if ((!(angle > shooterManager.maxAimAngle || angle < -shooterManager.maxAimAngle)) || controlAimCanvas && controlAimCanvas.isScopeCameraActive)
                {
                    upperArmRotation = Quaternion.Lerp(upperArmRotation, upperArmRotationAlignment, shooterManager.smoothArmIKRotation * (.001f + Time.deltaTime));
                }
                else
                {
                    upperArmRotation = Quaternion.Euler(0, 0, 0);
                }

                if (!float.IsNaN(upperArmRotation.x) && !float.IsNaN(upperArmRotation.y) && !float.IsNaN(upperArmRotation.z))
                    upperArm.localRotation *= Quaternion.Euler(upperArmRotation.eulerAngles.NormalizeAngle() * armAlignmentWeight);
            }
            else
            {
                upperArmRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        protected virtual void RotateAimHand(bool isUsingLeftHand = false)
        {
            if (!shooterManager) return;

            if (CurrentActiveWeapon && armAlignmentWeight > 0.1f && aimConditions && CurrentActiveWeapon.alignRightHandToAim)
            {
                var aimPoint = targetArmAlignmentPosition;
                Vector3 v = aimPoint - CurrentActiveWeapon.aimReference.position;
                var orientation = CurrentActiveWeapon.aimReference.forward;
                var hand = isUsingLeftHand ? leftHand : rightHand;
                var rot = Quaternion.FromToRotation(hand.InverseTransformDirection(orientation), hand.InverseTransformDirection(v));
                if (!shooterManager.isShooting &&
                    (!float.IsNaN(rot.x) && !float.IsNaN(rot.y) && !float.IsNaN(rot.z)))
                    handRotationAlignment = rot;
                var angle = Vector3.Angle(aimPosition - aimAngleReference.transform.position, aimAngleReference.transform.forward);
                if ((!(angle > shooterManager.maxAimAngle || angle < -shooterManager.maxAimAngle)) || (controlAimCanvas && controlAimCanvas.isScopeCameraActive))
                    handRotation = Quaternion.Lerp(handRotation, handRotationAlignment, shooterManager.smoothArmIKRotation * (.001f + Time.deltaTime));
                else handRotation = Quaternion.Euler(0, 0, 0);

                if (!float.IsNaN(handRotation.x) && !float.IsNaN(handRotation.y) && !float.IsNaN(handRotation.z))
                    hand.localRotation *= Quaternion.Euler(handRotation.eulerAngles.NormalizeAngle() * armAlignmentWeight);

                CurrentActiveWeapon.SetScopeLookTarget(aimPoint);
            }
            else handRotation = Quaternion.Euler(0, 0, 0);
        }

        #region Old Rotate Arm system
        //protected virtual void RotateAimArm(bool isUsingLeftHand = false)
        //{
        //    if (!shooterManager) return;

        //    if (CurrentActiveWeapon && (isAiming || aimTimming > 0f) && aimConditions && CurrentActiveWeapon.alignRightUpperArmToAim)
        //    {
        //        var aimPoint = targetArmAlignmentPosition;
        //        Vector3 v = aimPoint - CurrentActiveWeapon.aimReference.position;
        //        Vector3 v2 = Quaternion.AngleAxis(-CurrentActiveWeapon.recoilUp, CurrentActiveWeapon.aimReference.right) * v;
        //        var orientation = CurrentActiveWeapon.aimReference.forward;
        //        armAlignmentWeight = Mathf.Lerp(armAlignmentWeight, !shooterManager.isShooting || CurrentActiveWeapon.ammoCount <= 0 ? 1f * aimWeight : 0f, 1f * Time.deltaTime);
        //        var upperArm = isUsingLeftHand ? leftUpperArm : rightUpperArm;
        //        var r = Quaternion.FromToRotation(orientation, v) * upperArm.rotation;
        //        var r2 = Quaternion.FromToRotation(orientation, v2) * upperArm.rotation;
        //        Quaternion rot = Quaternion.Lerp(r2, r, armAlignmentWeight);
        //        var angle = Vector3.Angle(aimPosition - aimAngleReference.transform.position, aimAngleReference.transform.forward);

        //        if ((!(angle > shooterManager.maxAimAngle || angle < -shooterManager.maxAimAngle)) || controlAimCanvas && controlAimCanvas.isScopeCameraActive)
        //        {
        //            upperArmRotation = Quaternion.Lerp(upperArmRotation, rot, shooterManager.smoothArmIKRotation * Time.deltaTime);
        //        }
        //        else upperArmRotation = upperArm.rotation;

        //        if (!float.IsNaN(upperArmRotation.x) && !float.IsNaN(upperArmRotation.y) && !float.IsNaN(upperArmRotation.z))
        //            upperArm.rotation = upperArmRotation;
        //    }
        //}

        //protected virtual void RotateAimHand(bool isUsingLeftHand = false)
        //{
        //    if (!shooterManager) return;

        //    if (CurrentActiveWeapon && CurrentActiveWeapon.alignRightHandToAim && (isAiming || aimTimming > 0f) && aimConditions)
        //    {
        //        var aimPoint = targetArmAlignmentPosition;
        //        Vector3 v = aimPoint - CurrentActiveWeapon.aimReference.position;
        //        Vector3 v2 = Quaternion.AngleAxis(-CurrentActiveWeapon.recoilUp, CurrentActiveWeapon.aimReference.right) * v;
        //        var orientation = CurrentActiveWeapon.aimReference.forward;

        //        if (!CurrentActiveWeapon.alignRightUpperArmToAim)
        //            armAlignmentWeight = Mathf.Lerp(armAlignmentWeight, !shooterManager.isShooting || CurrentActiveWeapon.ammoCount <= 0 ? 1f * aimWeight : 0f, 1f * Time.deltaTime);

        //        var hand = isUsingLeftHand ? leftHand : rightHand;
        //        var r = Quaternion.FromToRotation(orientation, v) * hand.rotation;
        //        var r2 = Quaternion.FromToRotation(orientation, v2) * hand.rotation;
        //        Quaternion rot = Quaternion.Lerp(r2, r, armAlignmentWeight);
        //        var angle = Vector3.Angle(aimPosition - aimAngleReference.transform.position, aimAngleReference.transform.forward);

        //        if ((!(angle > shooterManager.maxAimAngle || angle < -shooterManager.maxAimAngle)) || (controlAimCanvas && controlAimCanvas.isScopeCameraActive))
        //            handRotation = Quaternion.Lerp(handRotation, rot, shooterManager.smoothArmIKRotation * Time.deltaTime);
        //        else handRotation = Quaternion.Lerp(hand.rotation, rot, shooterManager.smoothArmIKRotation * Time.deltaTime);

        //        if (!float.IsNaN(handRotation.x) && !float.IsNaN(handRotation.y) && !float.IsNaN(handRotation.z))
        //            hand.rotation = handRotation;

        //        CurrentActiveWeapon.SetScopeLookTarget(aimPoint);
        //    }
        //}
        #endregion

        protected virtual void CheckAimConditions()
        {
            if (!shooterManager) return;
            var weaponSide = isCameraRightSwitched ? -1 : 1;

            if (CurrentActiveWeapon == null)
            {
                aimConditions = false;
                return;
            }
            if (!shooterManager.hipfireShot && !IsAimAlignWithForward())
            {
                aimConditions = false;
            }
            else
            {
                var _ray = new Ray(aimAngleReference.transform.position + transform.up * shooterManager.blockAimOffsetY + transform.right * shooterManager.blockAimOffsetX * weaponSide, Camera.main.transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(_ray, shooterManager.checkAimRadius, out hit, shooterManager.minDistanceToAim, shooterManager.blockAimLayer))
                {
                    aimConditions = false;
                }
                else
                    aimConditions = true;
            }

            aimWeight = Mathf.Lerp(aimWeight, aimConditions ? 1 : 0, 10 * Time.deltaTime);
        }

        protected virtual bool IsAimAlignWithForward()
        {
            if (!shooterManager) return false;
            var angle = Quaternion.LookRotation(aimPosition - aimAngleReference.transform.position, Vector3.up).eulerAngles - transform.eulerAngles;

            return ((angle.NormalizeAngle().y < 90 && angle.NormalizeAngle().y > -90));
        }

        protected virtual Vector3 targetArmAlignmentPosition
        {
            get
            {
                return isUsingScopeView && controlAimCanvas.scopeCamera ? Camera.main.transform.position + Camera.main.transform.forward * lastAimDistance : aimPosition;
            }
        }

        protected virtual Vector3 targetArmAligmentDirection
        {
            get
            {
                var t = controlAimCanvas && controlAimCanvas.isScopeCameraActive && controlAimCanvas.scopeCamera ? controlAimCanvas.scopeCamera.transform : Camera.main.transform;
                return t.forward;
            }
        }

        protected virtual void UpdateHeadTrack()
        {
            if (!shooterManager || !headTrack)
            {
                if (headTrack) headTrack.offsetSpine = Vector2.Lerp(headTrack.offsetSpine, Vector2.zero, headTrack.smooth * Time.deltaTime);
                return;
            }
            if (!CurrentActiveWeapon || !headTrack)
            {
                if (headTrack) headTrack.offsetSpine = Vector2.Lerp(headTrack.offsetSpine, Vector2.zero, headTrack.smooth * Time.deltaTime);
                return;
            }
            if (isAiming || aimTimming > 0f)
            {
                var offset = cc.isCrouching ? CurrentActiveWeapon.headTrackOffsetCrouch : CurrentActiveWeapon.headTrackOffset;
                headTrack.offsetSpine = Vector2.Lerp(headTrack.offsetSpine, offset, headTrack.smooth * Time.deltaTime);
            }
            else
            {
                headTrack.offsetSpine = Vector2.Lerp(headTrack.offsetSpine, Vector2.zero, headTrack.smooth * Time.deltaTime);
            }
        }

        protected virtual void UpdateAimHud()
        {
            if (!shooterManager || !controlAimCanvas) return;
            if (CurrentActiveWeapon == null) return;
            controlAimCanvas.SetAimCanvasID(CurrentActiveWeapon.scopeID);
            if (controlAimCanvas.scopeCamera && controlAimCanvas.scopeCamera.gameObject.activeSelf)
                controlAimCanvas.SetAimToCenter(true);
            else if (isAiming)
            {
                RaycastHit hit;
                if (Physics.Linecast(CurrentActiveWeapon.muzzle.position, aimPosition, out hit, shooterManager.blockAimLayer))
                    controlAimCanvas.SetWordPosition(hit.point, aimConditions);
                else
                    controlAimCanvas.SetWordPosition(aimPosition, aimConditions);
            }
            else
                controlAimCanvas.SetAimToCenter(true);

            if (CurrentActiveWeapon.scopeTarget)
            {
                var lookPoint = Camera.main.transform.position + (Camera.main.transform.forward * (isUsingScopeView ? lastAimDistance : 100f));
                controlAimCanvas.UpdateScopeCamera(CurrentActiveWeapon.scopeTarget.position, lookPoint, CurrentActiveWeapon.zoomScopeCamera ? 0 : CurrentActiveWeapon.scopeZoom);
            }
        }

        #endregion
    }
}