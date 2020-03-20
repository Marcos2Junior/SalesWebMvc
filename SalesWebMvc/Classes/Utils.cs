using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Classes
{
    public class Utils
    {
        /// <summary>
        /// delete image
        /// </summary>
        /// <param name="typeFolder">name of folder</param>
        /// <param name="image">name of image</param>
        public void RemoveImage(string typeFolder, string image)
        {
            try
            {
                File.Delete(Directory.GetCurrentDirectory()
               + "\\wwwroot\\images\\" + typeFolder + "\\" + image);
                //testando
            }
            catch (Exception)
            {
            }
        }
    }
}
