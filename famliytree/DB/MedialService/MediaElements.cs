using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DB
{
    public class MediaElements
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }
        public bool isPublic { get; set; }
    }

    public class MediaService: BaseModel
    {
        DataTable o =  DbUtility.GetInstance().CallProcedure("",null);

        public IList<MediaElements> GetUnpublisMediaElements()
        {

            DataTable o = DB.DbUtility.GetInstance().CallProcedure("", null) ;
            return ConvertTo<MediaElements>(o);
        }

    }
}
