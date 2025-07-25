using System.Collections.Generic;

namespace Game.Hot
{
    public partial class BattleProperty
    {
        public int this[EPropertyType type]
        {
            get
            {
                var idx = (int)type;
                if (idx < 0 || idx >= _properties.Count)
                    return 0;
                return _properties[idx];
            }
        }
        
        private List<int> _properties;
        
        public BattleProperty()
        {
            _properties = new List<int>((int)EPropertyType.Max);
            for (int i = 0; i < (int)EPropertyType.Max; i++)
            {
                _properties.Add(0);
            }
        }


    }
}