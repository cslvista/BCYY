using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BCYY.common
{
    class CommonData
    {
        public static DataTable Room_FE()
        {
            DataTable Room = new DataTable();

            Room.Columns.Add("Room");
            Room.Columns.Add("Room_ID");
            Room.Rows.Add(new object[] {"标准房","0"});
            Room.Rows.Add(new object[] { "VIP套房", "1" });
            Room.Rows.Add(new object[] { "VIP豪华套房", "2" });
            Room.Rows.Add(new object[] { "VIP豪华套房(LD2)", "3" });
            return Room;
        }


        public static DataTable Room_FY()
        {
            DataTable Room = new DataTable();
            Room.Columns.Add("Room");
            Room.Columns.Add("Room_ID");
            Room.Rows.Add(new object[] { "单人房", "0" });
            Room.Rows.Add(new object[] { "标准房", "1" });
            Room.Rows.Add(new object[] { "VIP套房", "2" });
            Room.Rows.Add(new object[] { "LDR套房", "3" });
            return Room;
        }

        public static DataTable Dep_FY()
        {
            DataTable Dep = new DataTable();
            Dep.Columns.Add("Dep");
            Dep.Columns.Add("Dep_ID");
            Dep.Rows.Add(new object[] { "产科", "A0030002" });
            Dep.Rows.Add(new object[] { "妇科", "A0030001" });
            return Dep;
        }

        public static DataTable Dep_FE()
        {
            DataTable Dep = new DataTable();
            Dep.Columns.Add("Dep");
            Dep.Columns.Add("Dep_ID");
            Dep.Rows.Add(new object[] { "产科", "A0030002" });
            Dep.Rows.Add(new object[] { "妇科", "A0030001" });
            Dep.Rows.Add(new object[] { "国际门诊", "A0030014" });
            Dep.Rows.Add(new object[] { "东湖门诊", "A0030017" });
            return Dep;
        }



    }
}
