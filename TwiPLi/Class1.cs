using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Net;

namespace TwiPLi
{
    public class Response
    {
        public readonly int StatusID;                  // like 1111
        public readonly int UserID;                     // like 11111
        public readonly string MediaID;             // like abc123
        public readonly string MediaUrl;            // Something like this http://twitpic.com/abc123
        public readonly string MiniImage;         // Mini Thumbnail URL
        public readonly string ThumbImage;     // Thumbnail URL
        public readonly bool Status;                  // True = Success | False= Fail :-(
        public readonly int ErrorCode;                 // err_code= 1001 - Invalid twitter username or passwor
        // err_code= 1002 - Image not found
        // err_code= 1003 - Invalid image type
        // err_code= 1004 - Image larger than 4MB 

        public readonly string ErrorMessage;     //Will contain the sorry excuse for not doing ur job ;-)
        //Invalid twitter username or password

        public Response(int statusid, int userid, string mediaid, string mediaurl) //to be called, when Method = UploadAndPost
        {
            StatusID = statusid;
            UserID = userid;
            MediaID = mediaid;
            MediaUrl = mediaurl;
            MiniImage = "http://twitpic.com/show/mini/" + mediaid;
            ThumbImage = "http://twitpic.com/show/thumb/" + mediaid; ;
            Status = true;
            ErrorCode = 0;
            ErrorMessage = "";
        }
        public Response(string mediaid, string mediaurl) //to be called, when Method = Upload
        {
            StatusID = 0;
            UserID = 0;
            MediaID = mediaid;
            MediaUrl = mediaurl;
            MiniImage = "http://twitpic.com/show/mini/" + mediaid;
            ThumbImage = "http://twitpic.com/show/thumb/" + mediaid; ;
            Status = true;
            ErrorCode = 0;
            ErrorMessage = "";
        }
        public Response(int errorcode, string message) //to be called, when error occurs
        {
            StatusID = -1;
            UserID = -1;
            MediaID = "";
            MediaUrl = "";
            MiniImage = "";
            ThumbImage = "";
            Status = false;
            ErrorCode = errorcode;
            ErrorMessage = message;
        }
        public Response(string message)
        {
            Status = false;
            ErrorCode = 0; //there is some internal problem
            ErrorMessage = message;
        }

        /*
         * An example success response
             <?xml version="1.0" encoding="UTF-8"?>
            <rsp status="ok">
                 <statusid>1111</statusid>
                 <userid>11111</userid>
                 <mediaid>abc123</mediaid>
                 <mediaurl>http://twitpic.com/abc123</mediaurl>
            </rsp>
         * 
         * A failed response
         * <?xml version="1.0" encoding="UTF-8"?>
            <rsp stat="fail">
                <err code="1001" msg="Invalid twitter username or password" />
            </rsp>
         */
    }

    public class TwitPicLib
    {
        private string Username; //The twitter username
        private string Password;   //The twitter password
        private KeyValuePair<string, string>[] MethodURIMapping;
        private IWebProxy _webProxy;
        public IWebProxy WebProxy
        {
            get { return _webProxy; }
            set { _webProxy = value; }
        }
        private string _userAgent;
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        public TwitPicLib(string user, string password)
        {
            MethodURIMapping = new KeyValuePair<string, string>[2];

            // URL for the TwitPic API's upload and post method
            MethodURIMapping[0] = new KeyValuePair<string, string>("UploadAndPost", "http://twitpic.com/api/uploadAndPost");
            // URL for the TwitPic API's upload method
            MethodURIMapping[1] = new KeyValuePair<string, string>("Upload", "http://twitpic.com/api/upload");

            Username = user;
            Password = password;
        }

        /// <summary>
        /// Uploads the photo and sends a new Tweet
        /// </summary>
        /// <param name="binaryImageData">The binary image data.</param>
        /// <param name="tweetMessage">The tweet message.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>Return true, if the operation was succeded.</returns>
        public Response UploadPhoto(byte[] binaryImageData, string tweetMessage, string filename)
        {
            try
            {
                // Documentation: http://www.twitpic.com/api.do
                string boundary = Guid.NewGuid().ToString();
                string requestUrl = String.IsNullOrEmpty(tweetMessage) ? MethodURIMapping[1].Value : MethodURIMapping[0].Value;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                if (!string.IsNullOrEmpty(_userAgent))
                    request.UserAgent = _userAgent;
                if (request.Proxy != null)
                {
                    request.Proxy = _webProxy;
                    request.UseDefaultCredentials = true;
                    request.Credentials = CredentialCache.DefaultCredentials;
                }
                string encoding = "iso-8859-1";

                request.PreAuthenticate = true;
                request.AllowWriteStreamBuffering = true;
                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                request.Method = "POST";

                string header = string.Format("--{0}", boundary);
                string footer = string.Format("--{0}--", boundary);

                StringBuilder contents = new StringBuilder();
                contents.AppendLine(header);

                //determing file content-type
                string FileContentType = GetFileContentType(filename);
                if (FileContentType == "")
                {
                    return (new Response("Unable to determine file content-type | Invalid image type")); //cannot identify file-type/Invalid image format
                }

                string fileContentType = FileContentType;
                string fileHeader = String.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"", "media", filename);
                string fileData = Encoding.GetEncoding(encoding).GetString(binaryImageData);

                contents.AppendLine(fileHeader);
                contents.AppendLine(String.Format("Content-Type: {0}", fileContentType));
                contents.AppendLine();
                contents.AppendLine(fileData);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "username"));
                contents.AppendLine();
                contents.AppendLine(this.Username);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "password"));
                contents.AppendLine();
                contents.AppendLine(this.Password);

                if (!String.IsNullOrEmpty(tweetMessage))
                {
                    contents.AppendLine(header);
                    contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "message"));
                    contents.AppendLine();
                    contents.AppendLine(tweetMessage);
                }

                contents.AppendLine(footer);

                byte[] bytes = Encoding.GetEncoding(encoding).GetBytes(contents.ToString());
                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    //send data
                    requestStream.Write(bytes, 0, bytes.Length);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            return RSPXMLParser(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response("Cannot upload data: " + ex.ToString());
            }
        }

        private Response RSPXMLParser(string XMLresponse)
        {
            XmlDocument XmlDoc = new XmlDocument();
            string status = null;
            int UserId;
            int StatusId;
            string MediaId;
            string MediaUrl;
            int ErrorCode;
            string ErrorMessage;

            try
            {
                XmlDoc.LoadXml(XMLresponse);
                XmlNodeList RootNode = XmlDoc.SelectNodes("rsp");
                if (RootNode.Item(0).Attributes["status"] != null)
					// status is used in uploadAndPost Response
                    status = RootNode.Item(0).Attributes["status"].Value;
                else
					// stat is used in upload Response
                    status = RootNode.Item(0).Attributes["stat"].Value;
                if (status == "ok")
                {
                    //<?xml version="1.0" encoding="UTF-8"?>
                    //<rsp stat[us]="ok">
                    //<statusid>1111</statusid>
                    //<userid>11111</userid>
                    //<mediaid>abc123</mediaid>
                    //<mediaurl>http://twitpic.com/abc123</mediaurl>
                    //</rsp>
                    MediaId = RootNode.Item(0).SelectNodes("mediaid").Item(0).InnerText;
                    MediaUrl = RootNode.Item(0).SelectNodes("mediaurl").Item(0).InnerText;
                    try
                    {
                        UserId = int.Parse(RootNode.Item(0).SelectNodes("userid").Item(0).InnerText);
                        StatusId = int.Parse(RootNode.Item(0).SelectNodes("statusid").Item(0).InnerText);
                        return new Response(StatusId, UserId, MediaId, MediaUrl);
                    }
                    catch
                    {
                        return new Response(MediaId, MediaUrl);
                    }
                }
                else 
                {
					//<rsp stat[us]="fail">
                    //<err code="1001" msg="Invalid twitter username or password" />
                    ErrorCode = int.Parse(RootNode.Item(0).SelectNodes("err").Item(0).Attributes["code"].InnerText);
                    ErrorMessage = RootNode.Item(0).SelectNodes("err").Item(0).Attributes["msg"].InnerText;
                    return new Response(ErrorCode, ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return new Response("XML Parse error: " + ex.ToString());
            }
        }

        private string GetFileContentType(string FileName)
        {
            if (FileName.EndsWith("jpg", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                return "image/jpeg";
            }
            else if (FileName.EndsWith("jpeg", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                return "image/jpeg";
            }
            else if (FileName.EndsWith("gif", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                return "image/gif";
            }
            else if (FileName.EndsWith("png", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                return "image/png";
            }
            else
            {
                return "";
            }
        }

        /// Method corresponding to TwitPic API UploadAndPost
        public Response UploadAndPost(string path, string Tweet)
        {
            string filename;
            if(File.Exists(path))
            {
                filename = Path.GetFileName(path);
                return UploadPhoto(File.ReadAllBytes(path), Tweet, filename);
            }
            else
            {
                return new Response(0, "File not found");
            }
        }

        /// Method corresponding to TwitPic API Upload
        public Response Upload(string path)
        {
            string filename;
            if (File.Exists(path))
            {
                filename = Path.GetFileName(path);
                return UploadPhoto(File.ReadAllBytes(path), "", filename);
            }
            else
            {
                return new Response(0, "File not found");
            }
        }
    }
}
