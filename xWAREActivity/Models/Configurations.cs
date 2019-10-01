using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xWAREActivity.Models
{
    public static class Configurations
    {
        public static string APITokenURL = "http://192.168.1.75:61595/token";
        public static string serveruri = "http://192.168.1.75:61595/";
        public static string [] supportedImageTypes = new[] { "PNG", "JPEG", "TIFF", "JPG" ,"GIF"};
        public static string[] supportedFileTypes = new[] { "TXT", "DOC", "DOCX", "PDF", "XLS", "XLSX","PTX","PPT","PPTX" };
    }
}