using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    public class AppendSpeedTrigger : SkillTrigger
    {
        public float magnification;

        private vThirdPersonController personController;
        private float appcrouchSpeed;
        private float approtationSpeed;
        private float apprunnintSpeed;
        private float appsprintSpeed;
        private float appwalkSpeed;


        public override ISkillTrigger Clone()
        {
            return new AppendSpeedTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                
                personController = controller.gameObject.GetComponent<vThirdPersonController>();
                appcrouchSpeed = personController.freeSpeed.crouchSpeed * magnification;
                approtationSpeed = personController.freeSpeed.rotationSpeed * magnification;
                apprunnintSpeed = personController.freeSpeed.runningSpeed * magnification;
                appsprintSpeed = personController.freeSpeed.sprintSpeed * magnification;
                appwalkSpeed = personController.freeSpeed.walkSpeed *magnification;
                personController.freeSpeed.crouchSpeed += appcrouchSpeed;
                personController.freeSpeed.rotationSpeed += approtationSpeed;
                personController.freeSpeed.runningSpeed += apprunnintSpeed;
                personController.freeSpeed.sprintSpeed += appsprintSpeed;
                personController.freeSpeed.walkSpeed += appwalkSpeed;
                m_IsExected = true;
                return m_IsExected;
            }
            return m_IsExected;
        }

        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);

            m_StartTime = float.Parse(values[1]);
            magnification = float.Parse(values[2]);
        }

        public override void Reset()
        {
            base.Reset();
            personController.freeSpeed.crouchSpeed -= appcrouchSpeed;
            personController.freeSpeed.rotationSpeed -= approtationSpeed;
            personController.freeSpeed.runningSpeed -= apprunnintSpeed;
            personController.freeSpeed.sprintSpeed -= appsprintSpeed;
            personController.freeSpeed.walkSpeed -= appwalkSpeed;
            personController = null;

        }
    }
}