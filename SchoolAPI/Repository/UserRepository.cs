﻿using SchoolAPI.Data;
using SchoolAPI.Repository.IRepository;

namespace SchoolAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolContext _db;

        public UserRepository(SchoolContext db)
        {
            _db = db;
        }

        public bool IsUser(string username, string password)
        {
            var users = _db.Users.ToList();
            return users.Where(u => u.UserName == username
            && u.Password == password).Count() > 0;
        }
    }
}
