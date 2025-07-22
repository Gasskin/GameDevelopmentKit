using CodeBind;
using UnityEngine;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class SelectHeroForm : AHotUIForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            using var canChooseHero = UGFList<int>.Create();
            HotEntry.Model.Room.GetCanChooseHero(canChooseHero);
            if (canChooseHero.Count > 0) 
            {
                for (int i = 0; i < canChooseHero.Count && i < 2; i++)
                {
                    var heroId = canChooseHero[i];
                    if (HotEntry.Tables.DTHero.DataMap.TryGetValue(heroId, out var hero))
                    {
                        if (i == 0) 
                        {
                            HeroName1TMPText.text = hero.Name;
                        }
                        else
                        {
                            HeroName2TMPText.text = hero.Name;
                        }
                    }
                }
            }
        }
    }
}