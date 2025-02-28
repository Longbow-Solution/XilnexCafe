using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Image
    {
        #region Promo Images for Banners and Posters

        public class PromoImageResponse
        {
            public List<PromoImage> PromoList { get; set; }
        }

        public enum PromotionDisplayType
        {
            BANNER,
            POSTER
        }

        public enum PromotionImageAction
        {
            Static,
            Hyperlink,
            OpenPopup,
            OpenScreen
        }

        public class PromoImage
        {
            public string Id { get;set; }
            public string Name { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string FileAddress { get; set; }
            public string ImageType { get; set; }
            public PromotionDisplayType ImageTypeEnum { get; set; }
            public string ImageUrl { get; set; }
            public string ActionType { get; set; }
            public PromotionImageAction ActionTypeEnum { get; set; }
            public string ActionOpenTargetId { get; set; } //Can store screen name, screen ID, popup ID, or hyperlink URL.
            public int ImageSequence { get; set; }

            public PromoImage() { }

            public PromoImage(string id, string fileName, string url)
            {
                //Image filename format to follow
                //poster-1.png
                //banner-5.png

                //Assuming the file name has .png or .jpeg, remove it.
                string noExtFileName = "";
                if (fileName.Contains("."))
                {
                    noExtFileName = fileName.Remove(fileName.IndexOf('.'));
                }
                string[] parsedData = noExtFileName.Split('-');
                if (parsedData == null)
                    parsedData = new string[0];

                this.Id = id;
                this.FileName = fileName;
                this.ImageUrl = url;

                if (parsedData.Length == 2)
                {
                    this.ImageType = parsedData[0];
                    this.ImageSequence = int.Parse(parsedData[1]);
                }
            }

            public PromoImage(string id, string fileName, string url, string imageType, int seq)
            {
                if (string.IsNullOrEmpty(imageType))
                    imageType = "BANNER";
                this.Id = id;
                this.FileName = fileName;
                this.ImageUrl = url;
                this.ImageType = imageType.ToLower();
                this.ImageSequence = seq;
                if (ImageType == "poster")
                {
                    ImageTypeEnum = PromotionDisplayType.POSTER;
                }
                else
                {
                    ImageTypeEnum = PromotionDisplayType.BANNER;
                }
            }

            public void SetAction(string type, string target)
            {
                if (string.IsNullOrEmpty(type) == false)
                {
                    ActionType = type;
                    ActionOpenTargetId = target;
                    ActionTypeEnum = (PromotionImageAction)Enum.Parse(typeof(PromotionImageAction), type);
                }
                else
                {
                    ActionType = PromotionImageAction.Static.ToString();
                }
            }
        }

        #endregion

        public class PrefetchImageResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public List<string> ImageUrls { get; set; }
            public PrefetchImageResponse()
            {
                if (ImageUrls == null)
                {
                    ImageUrls = new List<string>();
                }
            }
        }

        #region Splash Media

        public enum SplashMediaType
        {
            None,
            Splashscreen,
            FirstTimeSequence,
        }

        public class SplashMediaItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string FileAddress { get; set; }
            public string MediaUrl { get; set; }
            public string DisplayText { get; set; }
            public bool IsVideo { get; set; }
            public int Sequence { get; set; }
            public SplashMediaType TypeEnum { get; set; }

            public SplashMediaItem() { }
        }
        
        public class SplashMediaResponse : ResponseBase
        {
            public SplashMediaItem SplashscreenMedia { get; set; }
            public List<SplashMediaItem> FirstTimeSequenceMedia { get; set; }
            public List<SplashMediaItem> MediaList { get; set; }

            public SplashMediaResponse()
            {
                if (FirstTimeSequenceMedia == null)
                    FirstTimeSequenceMedia = new List<SplashMediaItem>();
                if (MediaList == null)
                    MediaList = new List<SplashMediaItem>();
            }
        }

        #endregion
    }
}