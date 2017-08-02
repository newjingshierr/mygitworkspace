
using System.Collections.Generic;
using Models.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;


namespace DB.FamliytreeArticle
{
   public class FamliytreeArticle: BaseModel
    {
        public IList<FamliyTreeArticleView> GetAll()
        {
           
            MySqlParameter[] parameters = { new MySqlParameter("@index", MySqlDbType.Int32,0),new MySqlParameter("@pageSize",MySqlDbType.Int32,20)};

           DataTable o = MySqlHelper.callProcedure("fy_get_all_article", parameters);
            return ConvertTo<FamliyTreeArticleView>(o);
        }

        public IList<FamliyTreeArticleView> GetMessagesByTitle(string title)
        {

            MySqlParameter[] parameters = { new MySqlParameter("@index", MySqlDbType.Int32, 0), new MySqlParameter("@pageSize", MySqlDbType.Int32, 20), new MySqlParameter("@title", title) };

            DataTable o = MySqlHelper.callProcedure("ft_get_message_by_title", parameters);
            return ConvertTo<FamliyTreeArticleView>(o);
        }


    }
}
