using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace App.Aplication
{
    public static class Utils
    {
        public static string GetParameter(string param, string defaultValue)
        {
            string stringValue = HttpContext.Current.Request.QueryString[param];
            if (null != stringValue)
            {
                stringValue = stringValue.Contains("?") ? stringValue.Split('?')[0] : stringValue;
                return stringValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string CurrentHost
        {
            get
            {
                return string.Concat("http://", HttpContext.Current.Request.Url.Authority);
            }
        }

        public static string GetBaseUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            }
        }

        public static bool ExistsCokiee(this HttpCookieCollection cookieCollection, string name)
        {
            if (cookieCollection == null)
            {
                throw new ArgumentNullException("cookieCollection");
            }
            return cookieCollection[name] != null;
        }

        public static bool ExistsFile(this HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                return true;
            }
            return false;
        }

        public static Guid GetGuid(string value)
        {
            Guid guid = new Guid();
            Guid.TryParse(value, out guid);
            return guid;
        }

        public static long GetTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            TimeSpan universalTime = DateTime.Now.ToUniversalTime() - dateTime;
            return (long)(universalTime.TotalMilliseconds + 0.5);
        }

        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            if (data == null)
            {
                return false;
            }
            return data.Any<T>();
        }

        public static string NonAccent(this string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                txt = string.Empty;
            }
            else
            {
                string[] strArrays = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
                string[] strArrays1 = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
                for (int i = 1; i < (int)strArrays.Length; i++)
                {
                    for (int j = 0; j < strArrays[i].Length; j++)
                    {
                        txt = txt.Replace(strArrays1[i][j], strArrays1[0][i - 1]).Replace(strArrays[i][j], strArrays[0][i - 1]);
                    }
                }
                txt = Regex.Replace(Regex.Replace(txt, "[^a-zA-Z0-9_-]", "-", RegexOptions.Compiled), "-+", "-", RegexOptions.Compiled).Trim(new char[] { '-' });
            }
            return txt.ToLower();
        }

        public static string TrimVietNamMark(this string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                txt = string.Empty;
            }
            else
            {
                string[] strArrays = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
                string[] strArrays1 = new string[] { "aAeEoOuUiIdDyY", "áàảãạăắằẳẵặâấầẩẫậ", "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ", "éèẻẽẹêếềểễệ", "ÉÈẺẼẸÊẾỀỂỄỆ", "óòỏõọôốồổỗộơớờởỡợ", "ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ", "úùủũụưứừửữự", "ÚÙỦŨỤƯỨỪỬỮỰ", "íìỉĩị", "ÍÌỈĨỊ", "đ", "Đ", "ýỳỷỹỵ", "ÝỲỶỸỴ" };
                for (int i = 1; i < (int)strArrays.Length; i++)
                {
                    for (int j = 0; j < strArrays[i].Length; j++)
                    {
                        txt = txt.Replace(strArrays1[i][j], strArrays1[0][i - 1]).Replace(strArrays[i][j], strArrays[0][i - 1]);
                    }
                }
            }
            return txt.ToLower();
        }

        public static string SplitWords(int length, string words)
        {
            if (string.IsNullOrEmpty(words))
            {
                return string.Empty;
            }
            if (words.Length < length)
            {
                return words;
            }
            return string.Concat(words.Substring(0, length), "...");
        }

        /// <summary>
        /// Format lại tên file, có dạng: fileName-123-456-789.png
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string FileNameFormat(this string fileName, string fileExtension)
        {
            if (fileName.Length > 250)
                fileName = SplitWords(250, fileName);

            return string.Format("{0}-{1}{2}", fileName.NonAccent(), Guid.NewGuid(), fileExtension);
        }

        public static string FolderName()
        {
            return string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
        }

        public static string GetPathImage(string pathImage)
        {
            string src = string.Empty, url = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(pathImage))
                {
                    src = HttpContext.Current.Server.MapPath(pathImage);
                    if (File.Exists(src))
                        url = Utils.CombineUrl(GetBaseUrl, src);
                    else
                        url = Utils.CombineUrl(GetBaseUrl, Contains.ImageNoExsits);
                }
                else
                    url = Path.Combine(GetBaseUrl, Contains.ImageNoExsits);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return url;
        }

        #region Url

        public static string CombineUrl(string baseUrl, string relativeUrl)
        {
            UriBuilder baseUri = new UriBuilder(baseUrl);
            Uri newUri = null;

            if (Uri.TryCreate(baseUri.Uri, relativeUrl, out newUri))
                return newUri.ToString();
            else
                throw new ArgumentException("Unable to combine specified url values");
        }

        #endregion
    }
}