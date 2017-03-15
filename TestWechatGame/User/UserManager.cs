﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWechatGame
{
    public class UserManager
    {

        public static User[] GetUsers()
        {
            string sql = "SELECT * FROM User";
            var reader = Database.Instance.ExecuteSQL(sql);
            List<User> users = new List<User>();
            while (reader.Read()) {
                Int64 id = (Int64)reader["UserID"];
                string name = (string)reader["Name"];
                User user = new User(name, id);
                users.Add(user);
            }
            return users.ToArray();
        }


        public static object GetUserData(Int64 uid, string dataName)
        {
            string sql = string.Format("SELECT {0} FROM GameData WHERE UserID = {1}",dataName,uid);
            var reader = Database.Instance.ExecuteSQL(sql);
            if (reader.FieldCount != 1) return null;
            if (reader.Read()) {
                return reader[dataName];
            }
            return null;
        }

        public static bool SetUserData(Int64 uid, string dataName, object data)
        {
            string sql = string.Format("UPDATE GameData SET {0} = {1} WHERE UserID = {2}",dataName,data,uid);
            return Database.Instance.ExecuteNoQuery(sql) == 1;
        }


        public static User CreateUser(string name)
        {
            // 插入数据
            string sql = string.Format("INSERT INTO User (Name) VALUES('{0}')",name);
            int count = Database.Instance.ExecuteNoQuery(sql);
            if (count != 1) return null;

            // 查询ID
            sql = "SELECT last_insert_rowid() as UserID";
            var reader = Database.Instance.ExecuteSQL(sql);
            if (!reader.Read()) return null;

            Int64 uid = (Int64)reader["UserID"];
           

            // 插入Data
            sql = string.Format("INSERT INTO GameData (UserID) VALUES({0})", uid);
            count = Database.Instance.ExecuteNoQuery(sql);
            if (count != 1) return null;

            User user = new User(name, uid);
            return user;
        }


    }
}
