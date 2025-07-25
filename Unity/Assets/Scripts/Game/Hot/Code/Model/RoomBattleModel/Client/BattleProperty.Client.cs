namespace Game.Hot
{
    public partial class BattleProperty
    {
        public void SetPropertyClient(EPropertyType type, int propertyValue)
        {
            _properties[(int)type] = propertyValue;
        }
    }
}