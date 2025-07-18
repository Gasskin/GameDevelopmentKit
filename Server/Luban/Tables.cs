using Luban;

namespace cfg
{
    public partial class Tables
    {
        public static Tables Instance { get; } = new((path => new ByteBuf(File.ReadAllBytes($"../../../Luban/Data/{path}.bytes"))));

        public void Init()
        {
            
        }
    }
}