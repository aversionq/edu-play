//using System;
//using EduPlay.DAL.Interfaces;
//using EduPlay.DAL.Entities;
//using EduPlay.BLL.Interfaces;
//using EduPlay.BLL.Models;
//using EduPlay.DAL;
//using EduPlay.BLL;

//namespace EduPlay.Dependencies
//{
//    public class DependencyResolver
//    {
//        private static DependencyResolver _instance;
//        private IEduPlayDAL _eduPlayDAL;
//        private IEduPlayBLL _eduplayBLL;
//        private DependencyResolver() { }

//        public static DependencyResolver Instance => _instance ??= new DependencyResolver();
//        public IEduPlayDAL EduPlayDAL => _eduPlayDAL ??= new EduPlayDAL();
//        public IEduPlayBLL EduPlayBLL => _eduplayBLL ??= new EduPlayBLL(EduPlayDAL);
//    }
//}
