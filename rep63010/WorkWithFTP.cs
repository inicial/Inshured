using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace FileFromToFTP_NEW
{
    class WorkWithFTP
    {
        //const
        public const int MAX_COUNT_TRY = 3; //максимальное кол-во попыток действий для FTP
        //enum
        public enum FTP_ERROR
        { 
            ERROR_NO,           //Нет ошибки
            ERROR_UKNOWN,       //Неизвестная ошибка
            ERROR_EMPTYPATH,    //Пустое имя папки(путевки)
            ERROR_FTPNOTFOUND,  //Не правильный адрес для Ftp сервера, или такой сервер не существует
            ERROR_FTP,          //Ошибка FTP сервера
            ERROR_FILENOTFOUND, //Не найден файл для загрузки на FTP
            ERROR_FTPINS,       //Ошибка FTP сервера при сохранении файла на FTP
            ERROR_FTPDEL        //Ошибка FTP сервера при удалении файла на FTP
        };
        private const string lala = "orderpaper";
        private const string papa = "5jm60i5bsk";
        private const string begin_uri = "ftp://";
        //private
        private string strFTP;
        //constructors
        public WorkWithFTP()
        {
            this.strFTP = WorkWithFTP.begin_uri;
        }
        public WorkWithFTP(string servername)
        {
            this.strFTP = WorkWithFTP.begin_uri + servername + "/";
        }
        //Получение списка файлов для FTP и создание директории
        public FTP_ERROR GetFilesOnFTPAndCreateNewDir(string namepath, out string strerror)
        {
            strerror = "";
            if (namepath == "")
            {
                strerror = string.Format("ERROR_EMPTYPATH");
                return FTP_ERROR.ERROR_EMPTYPATH;
            }
            Uri serverUri = new Uri(this.strFTP);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                strerror = string.Format("ERROR_FTPNOTFOUND: serverUri.Scheme={0}, Uri.UriSchemeFtp={1}", serverUri.Scheme, Uri.UriSchemeFtp);
                return FTP_ERROR.ERROR_FTPNOTFOUND;
            }
            FtpWebRequest reqFTP;
            WebResponse response;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                //reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                IWebProxy myProxy = GlobalProxySelection.GetEmptyWebProxy();
                GlobalProxySelection.Select = myProxy;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(lala, papa);
                //список директорий(папок, путевок) в корне на FTP
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                bool flag = false;
                while (line != null)
                {
                    if (line == namepath)
                        flag = true;
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                //создание директории(папок, путевок) на FTP, если не существует
                serverUri = new Uri(this.strFTP + namepath);
                reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(lala, papa);
                if (!flag)
                {
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    response = reqFTP.GetResponse();
                    response.Close();
                }
                return FTP_ERROR.ERROR_NO;
            }
            catch (Exception ex)
            {
                strerror = string.Format("Получение списка файлов для FTP и создание директории: {0}", ex.Message);
                return FTP_ERROR.ERROR_FTP;
            }
        }
        //загрузка файла на FTP с диска
        public FTP_ERROR Upload(string namepath, string fileName, string newnamefile, out string strerror)
        {
            //провекри
            strerror = "";
            if (namepath == "")
            {
                return FTP_ERROR.ERROR_EMPTYPATH;
            }
            if ((fileName == "") || (!File.Exists(fileName)))
            {
                return FTP_ERROR.ERROR_FILENOTFOUND;
            }
            FileInfo fileInf = new FileInfo(fileName);
            Uri serverUri = new Uri(this.strFTP + namepath + "/" + newnamefile + fileInf.Extension);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return FTP_ERROR.ERROR_FTPNOTFOUND;
            }
            //копирование
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                //reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                IWebProxy myProxy = GlobalProxySelection.GetEmptyWebProxy();
                GlobalProxySelection.Select = myProxy;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(lala, papa);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.ContentLength = fileInf.Length;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                //сохранение файла
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buffer, 0, bufferSize);
                while (contentLen != 0)
                {
                    strm.Write(buffer, 0, contentLen);
                    contentLen = fs.Read(buffer, 0, bufferSize);
                }
                strm.Close();
                fs.Close();
                return FTP_ERROR.ERROR_NO;
            }
            catch (Exception ex)
            {
                strerror = string.Format("Загрузка файла на FTP с диска: {0}", ex.Message);
                return FTP_ERROR.ERROR_FTPINS;
            }
        }
        //загузка файла на диск с Ftp
        public FTP_ERROR Download(string namepath, string fileName, string filePath, out string strerror, out string file)
        {
            //провекри
            strerror = "";
            file = filePath + "\\" + fileName;
            if (namepath == "")
            {
                return FTP_ERROR.ERROR_EMPTYPATH;
            }
            if (fileName == "")
            {
                return FTP_ERROR.ERROR_FILENOTFOUND;
            }
            FileInfo fileInf = new FileInfo(fileName);
            Uri serverUri = new Uri(this.strFTP + namepath + "/" + fileInf.Name);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return FTP_ERROR.ERROR_FTPNOTFOUND;
            }
            //загрузка файла
            FtpWebRequest reqFTP;
            FtpWebResponse response;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileInf.Name, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                //reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                IWebProxy myProxy = GlobalProxySelection.GetEmptyWebProxy();
                GlobalProxySelection.Select = myProxy;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(lala, papa);
                response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return FTP_ERROR.ERROR_NO;
            }
            catch (Exception ex)
            {
                strerror = string.Format("Загузка файла на диск с Ftp: {0}", ex.Message);
                return FTP_ERROR.ERROR_FTP;
            }

        }
        //удаление файла с Ftp
       public FTP_ERROR Delete(string namepath, string fileName, out string strerror)
        {
            strerror = "";
            //провекри
            if (namepath == "")
            {
                return FTP_ERROR.ERROR_EMPTYPATH;
            }
            if (fileName == "")
            {
                return FTP_ERROR.ERROR_FILENOTFOUND;
            }
            FileInfo fileInf = new FileInfo(fileName);
            Uri serverUri = new Uri(this.strFTP + namepath);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return FTP_ERROR.ERROR_FTPNOTFOUND;
            }
            //удаление
            FtpWebRequest reqFTP;
            FtpWebResponse response;
            try
            {
                //список файлов
                reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                //reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                IWebProxy myProxy = GlobalProxySelection.GetEmptyWebProxy();
                GlobalProxySelection.Select = myProxy;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(lala, papa);
                //список директорий(папок, путевок) в корне на FTP
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                response = (FtpWebResponse)reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                bool flag = false;
                while (line != null)
                {
                    if (line == namepath + "/" + fileInf.Name)
                        flag = true;
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                //удаление
                if (flag)
                {
                    serverUri = new Uri(this.strFTP + namepath + "/" + fileInf.Name);
                    reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                    //reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(lala, papa);
                    response = (FtpWebResponse)reqFTP.GetResponse();
                    response.Close();
                }
                return FTP_ERROR.ERROR_NO;
            }
            catch (Exception ex)
            {
                strerror = string.Format("Удаление файла с Ftp: {0}", ex.Message);
                return FTP_ERROR.ERROR_FTPDEL;
            }
        }
    }
}
