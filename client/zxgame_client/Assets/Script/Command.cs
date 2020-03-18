using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    public static class Command
    {
        public static string IsLogin(string username,string password)
        {
            string str = "login|" + username + "," + password;
            return str;
        }

        public static string CreateRoom(string creater, int playnum, int type, int style,string lastname)
        {
            string str = "create|" + creater + "," + playnum + "," + type + "," + style+","+lastname;
            return str;
        }

        public static string startGame(int roomid)
        {
            string str = "startGame|"+roomid;
            return str;
        }

        public static string IsJoinGame(int roomid,int type)
        {
            string str = "Isjoin|" + roomid+","+type;
            return str;
        }

        public static string JoinGame(string player, int roomid, string lastname)
        {
            string str = "join|" + player + "," + roomid + "," + lastname;
            return str;
        }

        public static string QuitRoom(string player,int roomid)
        {
            string str = "Quitroom|"+player+","+roomid;
            return str;
        }

        public static string RoomInfo(string roomid)
        {
            string str = "RoomInfo|"+roomid;
            return str;
        }

        public static string Talk(string lastname,string msg,string roomid)
        {
            string str = "msg|" + lastname+","+msg+","+roomid;
            return str;
        }

    }
}
