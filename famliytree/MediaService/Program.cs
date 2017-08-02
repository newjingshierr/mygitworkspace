using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;


namespace MediaService
{
    /*
     * 1、根据上传记录，查询未发布的视频；
     * 2、获取ts的文件；
     * 3、ts文件进行编码；
     * 4、发布生成URL地址；
     * 5、替换生成的地址；
     */
   public class Program
    {
        static void Main(string[] args)
        {
            //*1、根据上传记录，查询未发布的视频；

            DB.MediaService mediaService = new DB.MediaService();
            var result = mediaService.GetUnpublisMediaElements();
            foreach(var item in result)
            {

            }
        }
    }

    
}
