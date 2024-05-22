using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Ho.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Ho
{
    public class Data
    {
        private static User _currentUser { get; set; }
        public static User currentUser
        {
            get
            {
                return Data._currentUser;
            }
            set
            {
                Data._currentUser = value;
                DependencyService.Get<IFileService>().CreateFile(JsonConvert.SerializeObject(Data.data));
            }
        }

        public static User[] usersList { get; set; }

        public static Dictionary<string, object> data =>
            new Dictionary<string, object>()
            {
                { "currentUser", currentUser },
                { "usersList", usersList }
            };

        public Data(User currentUser, User[] usersList)
        {
            Data.currentUser = currentUser;
            Data.usersList = usersList ?? new User[]{};
        }

        public static void AddUserToList(User user)
        {
            if (Data.usersList is null)
            {
                Data.usersList = new User[] { };
            }

            try
            {
                Data.usersList = Data.usersList.Append(user).ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
               
                DependencyService.Get<IFileService>().CreateFile(JsonConvert.SerializeObject(Data.data));
            }
        }
    }
}