using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;


namespace GameSystem
{
    public class SocialDialogSystem : ComponentSystem
    {

        public enum SocialDialogType
        {
            Description = 1,       //说明
            BasicInformation = 2,  //基础信息
            MainInformation = 3   //主要信息
        }


        public class DialogData
        {
            public int Id;
            public int[] parentIds = new int[0];
            public int[] childIds = new int[0];
            public string Content;
        }

        private static Dictionary<int, DialogData> _dialogDic = new Dictionary<int, DialogData>();




        public static void SetupComponentData(EntityManager entityManager)
        {
            List<SocialDialogData> socialDialogDatas = SqlData.GetAllDatas<SocialDialogData>();

            for (int i = 0; i < socialDialogDatas.Count; i++)
            {
                if (socialDialogDatas[i].Type == 1)
                {
                    GameStaticData.SocialDialogInfo.Add(socialDialogDatas[i].Id, socialDialogDatas[i].Content);
                }
                else if (socialDialogDatas[i].Type == 2)
                {
                    GameStaticData.SocialDialogNarration.Add(socialDialogDatas[i].Id, socialDialogDatas[i].Content);
                }

                DialogData dialogData = new DialogData();
                dialogData.Id = socialDialogDatas[i].Id;
                dialogData.Content = socialDialogDatas[i].Content;

                string[] childstr = socialDialogDatas[i].ChildId.Split(',');
                int[] childint = new int[childstr.Length];

                for (int j = 0; j < childstr.Length; j++)
                    childint[j] = Int32.Parse(childstr[j]);
                dialogData.childIds = childint;

                string[] parentstr = socialDialogDatas[i].ParentId.Split(',');
                int[] parentint = new int[parentstr.Length];

                for (int j = 0; j < parentstr.Length; j++)
                    parentint[j] = Int32.Parse(parentstr[j]);

                dialogData.parentIds = parentint;
            }
        }

        protected override void OnUpdate()
        {

        }

        public void Exchange(Entity a, Entity b)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            InteractionElement interactionA = entityManager.GetComponentData<InteractionElement>(a);
            InteractionElement interactionB = entityManager.GetComponentData<InteractionElement>(b);

            if (interactionA.Type == ElementType.Biological && interactionB.Type == ElementType.Biological)
            {
                Biological biologicalA = entityManager.GetComponentData<Biological>(a);
                
                

            }
            else if (interactionA.Type == ElementType.Biological && interactionB.Type == ElementType.LivingArea)
            {

            }



        }





    }


}
