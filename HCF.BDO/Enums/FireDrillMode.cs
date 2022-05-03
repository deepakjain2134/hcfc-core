using System.ComponentModel;

namespace HCF.BDO.Enums
{
    public enum FireDrillMode
    {
        [Description("By Buildings")]
        ByBuildings = 0,
        [Description("By Location Group")]
        ByLocationGroup = 1,
    }
}
