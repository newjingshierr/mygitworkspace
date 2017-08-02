using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DBModel;
using Models.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Reflection;

namespace DB
{
   public class BaseModel
    {
        public  IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
                return null;

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
                rows.Add(row);

            return ConvertTo<T>(rows);
        }

        public  IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public  T CreateItem<T>(DataRow row)
        {
            string columnName;
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    //Get property with same columnName
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    try
                    {
                        if (prop != null)/*数据库表字段和model不不匹配时候，采用model enzo*/
                        {
                            //Get value for the column
                            object value = (row[columnName].GetType() == typeof(DBNull))
                        ? null : row[columnName];
                            //Set property value

                            if (prop.CanWrite)    //判断其是否可写
                                prop.SetValue(obj, value, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                        //Catch whatever here
                    }
                }
            }
            return obj;
        }


    }
}
