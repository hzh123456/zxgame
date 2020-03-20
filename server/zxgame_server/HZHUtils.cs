using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    class HZHUtils
    {

        public static MySqlConnection conn;

        //解析用户发送消息的函数
        public static String JieXiMsg(string msg,out string[] data)
        {
            string[] strs = msg.Split('|');
            string command = strs[0];
            data = strs[1].Split(',');
            return command;
        }

        public static bool UpdateRoom(string roomid)
        {
            bool flag = false;

            Open();

            string sql = "update room set style=1 where id=@roomid";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@roomid", roomid));
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                flag = true;
            }
            Close();
            return flag;
        }

        public static bool CreateRoom(string creater, int playernum, int type, int style)
        {
            bool flag = false; 

            Open();

            string sql = "insert into room(creater,playernum,type,style) values(@creater,@playernum,@type,@style)";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@creater", creater));
            cmd.Parameters.Add(new MySqlParameter("@playernum", playernum));
            cmd.Parameters.Add(new MySqlParameter("@type", type));
            cmd.Parameters.Add(new MySqlParameter("@style", style));
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                flag = true;
            }
            Close();
            return flag;
        }

        public static int QuitRoom(string creater)
        {
            int roomid = -1;
             
            Open();
            string sql = "select * from room where creater=@creater";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@creater", creater));
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Close();
            }
            else
            {
                reader.Read();
                roomid = reader.GetInt32(0);
                Close();
                RemoveRoom(roomid);
            }

            return roomid;
        }

        public static List<T> DaLuanList<T>(List<T> list)
        { 
            var random = new Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }

        public static bool RemoveRoom(int roomid)
        {
            bool flag = false;

            Open();

            string sql = "delete from room where id=@roomid";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@roomid", roomid));
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                flag = true;
            }
            Close();
            return flag;
        }

        public static string IsRoom(int roomid, int type)
        {
            string str;

            Open();
            string sql = "select * from room where id=@roomid and type=@type";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@roomid", roomid));
            cmd.Parameters.Add(new MySqlParameter("@type", type));
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                str = "False";
            }
            else
            {
                reader.Read();
                int style = reader.GetInt32(4);
                if (style == 0)
                {
                    str = "True";
                }
                else
                {
                    str = "Playing";
                }
            }
            Close();

            return str;
        }

        public static bool GetRoomId(string creater,out int roomid)
        {
            bool flag = true;

            Open();
            string sql = "select * from room where creater=@creater";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@creater", creater));
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                roomid = -1;
                flag = false;
            }
            else
            {
                reader.Read();
                roomid = reader.GetInt32(0);
            }
            Close();

            return flag;
        }

        public static bool AddUser(string username, string password, string lastname)
        {
            bool flag = false;

            Open();

            string sql = "insert into user(username,password,lastname) values(@username,@pwd,@lastname)";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递

            cmd.Parameters.Add(new MySqlParameter("@username", username));
            cmd.Parameters.Add(new MySqlParameter("@pwd", password));
            cmd.Parameters.Add(new MySqlParameter("@lastname", lastname));
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            if(i>0){
                flag = true;
            }
            Close();
            return flag;
        }

        public static bool IsLogin(string username, string password,out string lastname)
        {
            bool flag = true;

            Open();
            string sql = "select * from user where username=@username and password=@pwd";//使用@符构造sql变量
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //使用MysqlCommand对象的parameters属性，该属性为像sql语句传递的参数集合，使用add方法向其中添加参数，参数以MysqlParameters对象形式传递
            
            cmd.Parameters.Add(new MySqlParameter("@username", username));
            cmd.Parameters.Add(new MySqlParameter("@pwd", password));
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                lastname = null;
                flag = false;
            }
            else
            {
                reader.Read();
                lastname = reader.GetString(2);
            }
            Close();

            return flag;
        }

        private static void Open()
        {
            string connstr = "data source=localhost;database=zxgame;user id=root;password=123456;pooling=false;charset=utf8";//pooling代表是否使用连接池
            conn = new MySqlConnection(connstr);
        }

        private static void Close()
        {
            conn.Close();
        }

    }
}
