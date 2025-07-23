using CodeBind;
using GameFramework;

namespace Game.Hot
{
    public class HeroCardWidgetOpenData : IReference
    {
        public int HeroId;
        
        public void Clear()
        {
            HeroId = 0;
        }

        public static HeroCardWidgetOpenData Create(int heroId)
        {
            var r = ReferencePool.Acquire<HeroCardWidgetOpenData>();
            r.HeroId = heroId;
            return r;
        }
    }
    
    public partial class HeroCardWidget: AUIWidget
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            ClearBind();
        }

        protected override void OnOpen(IReference userData)
        {
            base.OnOpen(userData);
            var r = (HeroCardWidgetOpenData)userData;
            var drHero = HotEntry.Tables.DTHero.GetOrDefault(r.HeroId);
            if (drHero != null)
            {
                HeroNameTMPText.text = drHero.Name;
                HandCardTMPText.text = drHero.HandLimit.ToString();
                HpTMPText.text = drHero.HpLimit.ToString();
            }
        }
    }
}