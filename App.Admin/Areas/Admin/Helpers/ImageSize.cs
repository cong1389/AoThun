using System;
using System.Collections.Specialized;
using System.Configuration;

namespace App.Admin.Helpers
{
	public static class ImageSize
	{
		public static int HeightBigSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightBigSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["HeightBigSize"]);
			}
		}

		public static int HeighthOrignalSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeighthOrignalSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["HeighthOrignalSize"]);
			}
		}

        public static int FlowStep_HeightMediumSize
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FlowStep_HeightMediumSize"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
                return int.Parse(ConfigurationManager.AppSettings["FlowStep_HeightMediumSize"]);
            }
        }

        public static int HeightMediumSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightMediumSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["HeightMediumSize"]);
			}
		}

		public static int HeightNewsMediumSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightNewsSize"]))
				{
					return 400;
				}
				return int.Parse(ConfigurationManager.AppSettings["HeightNewsSize"]);
			}
		}

		public static int HeightNewsSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightNewsSize"]))
				{
					return 450;
				}
				return int.Parse(ConfigurationManager.AppSettings["HeightNewsSize"]);
			}
		}

		public static int HeightPostRelativeSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightPostRelativeSize"]))
				{
					return 384;
				}
				return int.Parse(ConfigurationManager.AppSettings["HeightPostRelativeSize"]);
			}
		}

		public static int HeightSmallSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightSmallSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["HeightSmallSize"]);
			}
		}

		public static int HeightThumbnailSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HeightThumbnailSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["HeightDefaulSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["HeightThumbnailSize"]);
			}
		}

		public static int WithBigSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithBigSize"]))
				{
					return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
				}
				return int.Parse(ConfigurationManager.AppSettings["WithBigSize"]);
			}
		}

		public static int WithMediumNewsSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithNewsSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithNewsSize"]);
			}
		}

		public static int WithMediumSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithMediumSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithMediumSize"]);
			}
		}

		public static int WithNewsSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithNewsSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithNewsSize"]);
			}
		}

		public static int WithOrignalSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithOrignalSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithOrignalSize"]);
			}
		}

        public static int FlowStep_WithMediumSize
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FlowStep_WithMediumSize"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
                return int.Parse(ConfigurationManager.AppSettings["FlowStep_WithMediumSize"]);
            }
        }

        public static int WithPostRelativeSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithPostRelativeSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithPostRelativeSize"]);
			}
		}

		public static int WithSmallSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithSmallSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithSmallSize"]);
			}
		}

		public static int WithThumbnailSize
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WithThumbnailSize"]))
				{
                    return int.Parse(ConfigurationManager.AppSettings["WithDefaultSize"]);
                }
				return int.Parse(ConfigurationManager.AppSettings["WithThumbnailSize"]);
			}
		}
	}
}