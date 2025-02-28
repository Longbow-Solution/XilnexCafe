using AnWRestAPI;
using AnWRestAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net.Http;

namespace AnWRestAPI
{
    public class GeneralFunc
    {
        #region Encryption

        public static string Encrypt(string Input)
        {
            //static string Password = "DFWebDesigner";
            //static string Salt = "FrameworkRENG"; 

            string Password = "R1C#S%";
            string Salt = "H3avEn3arth";

            if (Input == null || Input.Length <= 0) return "";

            Rfc2898DeriveBytes keyGen = __createKeyGen(Password, Salt);
            ICryptoTransform transformer = __createEncryptor(keyGen);
            byte[] transformed = __transform(Encoding.Default.GetBytes(Input), transformer);

            return Convert.ToBase64String(transformed);
        }

        static Rfc2898DeriveBytes __createKeyGen(string Password, string Salt)
        {
            return new Rfc2898DeriveBytes(Password, Encoding.Default.GetBytes(Salt));
        }

        static ICryptoTransform __createEncryptor(Rfc2898DeriveBytes KeyGen)
        {
            TripleDES provider = TripleDES.Create();
            return provider.CreateEncryptor(KeyGen.GetBytes(16), KeyGen.GetBytes(16));
        }

        static ICryptoTransform __createDecryptor(Rfc2898DeriveBytes KeyGen)
        {
            TripleDES provider = TripleDES.Create();
            return provider.CreateDecryptor(KeyGen.GetBytes(16), KeyGen.GetBytes(16));
        }

        static byte[] __transform(byte[] Input, ICryptoTransform Transformer)
        {
            MemoryStream ms = new MemoryStream();
            byte[] result;

            CryptoStream writer = new CryptoStream(ms, Transformer, CryptoStreamMode.Write);
            writer.Write(Input, 0, Input.Length);
            writer.FlushFinalBlock();

            ms.Position = 0;
            result = ms.ToArray();

            ms.Close();
            writer.Close();
            return result;
        }

        public static string GetEncryptedString(string text)
        {
            //Copied from LoyaltyController.cs - DoEncrypt
            byte[] arrbyte = new byte[text.Length];
            try
            {
                SHA256 hash = new SHA256CryptoServiceProvider();
                arrbyte = hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
            catch (Exception)
            {
                
            }
            return Convert.ToBase64String(arrbyte);
        }

        static readonly char[] templateChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        public static string GenerateRandomPassword(int targetLength = 10)
        {
            //Reference: https://stackoverflow.com/a/1344255
            byte[] bufferBits = new byte[4 * targetLength];
            using (RandomNumberGenerator randomer = RandomNumberGenerator.Create())
            {
                randomer.GetBytes(bufferBits);
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < targetLength; i++)
            {
                var rnd = BitConverter.ToUInt32(bufferBits, i * 4);
                var idx = rnd % templateChars.Length;
                result.Append(templateChars[idx]);
            }
            return result.ToString();
        }

        #endregion

        #region Formatting check and get

        /// <summary>
        /// Validate email address format. Does not validate whether the email address is real or fake.
        /// </summary>
        /// <param name="givenEmailAddress">Email address input.</param>
        /// <returns>Boolean value whether email address is valid or not.</returns>
        public static bool ValidateEmailFormat(string givenEmailAddress)
        {
            if (string.IsNullOrWhiteSpace(givenEmailAddress))
            {
                return false;
            }
            else
            {
                givenEmailAddress = givenEmailAddress.Trim();
            }
            string regex = @"^(([^<>()[\]\\.,;:\s@']+(\.[^<> ()[\]\\.,;:\s@']+)*)|('.+ '))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
            return System.Text.RegularExpressions.Regex.IsMatch(givenEmailAddress, regex);
        }
        /// <summary>
        /// Get the specific phone number format.
        /// </summary>
        /// <param name="givenPhoneNum">Provided phone number to format.</param>
        /// <returns>Phone number string that starts with "6"</returns>
        public static string GetFormattedPhoneNumber(string givenPhoneNum)
        {
            if (givenPhoneNum.Contains("+"))
            {
                givenPhoneNum = givenPhoneNum.Replace("+", "");
            }
            if (givenPhoneNum.Contains("-"))
            {
                givenPhoneNum = givenPhoneNum.Replace("-", "");
            }
            if (givenPhoneNum.Contains(" "))
            {
                givenPhoneNum = givenPhoneNum.Replace(" ", "");
            }
            if (givenPhoneNum.StartsWith("0"))
            {
                givenPhoneNum = string.Format("6{0}", givenPhoneNum);
            }

            return givenPhoneNum;
        }
        /// <summary>
        /// Convert any Malaysian phone number format to ITU compliant format, E.164.
        /// </summary>
        /// <param name="givenPhoneNum">Phone number.</param>
        /// <returns>ITU compliant format. Starts with "+60" and no dashes.</returns>
        public static string GetISOMalaysianPhoneNumber(string givenPhoneNum)
        {
            //Remove any plus, dash, and spacing
            if (givenPhoneNum.Contains("+"))
            {
                givenPhoneNum = givenPhoneNum.Replace("+", "");
            }
            if (givenPhoneNum.Contains("-"))
            {
                givenPhoneNum = givenPhoneNum.Replace("-", "");
            }
            if (givenPhoneNum.Contains(" "))
            {
                givenPhoneNum = givenPhoneNum.Replace(" ", "");
            }
            //Re-add a plus symbol in-front if any. Check if the phone number starts with "0" or "60".
            if (givenPhoneNum.StartsWith("0"))
            {
                givenPhoneNum = string.Format("+6{0}", givenPhoneNum);
            }
            else if (givenPhoneNum.StartsWith("60"))
            {
                givenPhoneNum = string.Format("+{0}", givenPhoneNum);
            }
            else
            {
                givenPhoneNum = string.Format("+60{0}", givenPhoneNum);
            }
            return givenPhoneNum;
        }
        /// <summary>
        /// Combine two addresses and remove any unnecessary commas.
        /// </summary>
        /// <param name="addressOne">First address.</param>
        /// <param name="addressTwo">Second address.</param>
        /// <returns></returns>
        public static string CombineTwoAddress(string addressOne, string addressTwo)
        {
            //Null check and trim if not null.
            if (string.IsNullOrEmpty(addressOne))
            {
                addressOne = string.Empty;
            }
            else
            {
                addressOne = addressOne.Trim();
            }
            if (string.IsNullOrEmpty(addressTwo))
            {
                addressTwo = string.Empty;
            }
            else
            {
                addressTwo = addressTwo.Trim();
            }
            //Check address one.
            if (addressOne.EndsWith(","))
            {
                addressOne = addressOne.Substring(0, addressOne.Length - 1);
            }
            //Check address two.
            if (addressTwo.StartsWith(","))
            {
                addressTwo = addressTwo.Substring(1, addressTwo.Length - 1);
            }
            return string.Format("{0},{1}", addressOne, addressTwo);
        }
        /// <summary>
        /// Combine first name and last name as one full name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <returns></returns>
        public static string GetFullName(string firstName, string lastName)
        {
            //Null check and trim if not null.
            if (string.IsNullOrEmpty(firstName))
            {
                firstName = string.Empty;
            }
            else
            {
                firstName = firstName.Trim();
            }
            if (string.IsNullOrEmpty(lastName))
            {
                lastName = string.Empty;
            }
            else
            {
                lastName = lastName.Trim();
            }
            return string.Format("{0} {1}", firstName, lastName);
        }
        /// <summary>
        /// Get the Malaysian Standard Time from UTC assigned datetime.
        /// </summary>
        /// <param name="givenDt">Given DateTime object.</param>
        /// <returns>DateTime value with GMT plus 8.</returns>
        public static DateTime ConvertUtcToMalaysianTime(DateTime givenDt)
        {
            switch (givenDt.Kind)
            {
                case DateTimeKind.Unspecified:
                    return givenDt.AddHours(8); //GMT +8
                case DateTimeKind.Utc:
                    return givenDt.ToLocalTime();
                case DateTimeKind.Local:
                    return givenDt;
                default:
                    return givenDt.AddHours(8); //GMT +8
            }
        }
        /// <summary>
        /// Attempts to correct the Date of Birth value.
        /// </summary>
        /// <param name="givenDob">Provided Date of Birth input.</param>
        /// <returns>Corrected Date of Birth with time set to 8:00am</returns>
        public static DateTime GetCorrectedDateOfBirth(DateTime givenDob)
        {
            if (givenDob != DateTime.MinValue)
            {
                //If the hour was set before 12pm.
                if (givenDob.Hour <= 12)
                {
                    givenDob = givenDob.Date + new TimeSpan(8, 0, 0);
                }
                //If the hour was set after 12pm.
                else if (givenDob.Hour > 12)
                {
                    givenDob = givenDob.Date.AddDays(1) + new TimeSpan(8, 0, 0);
                }
            }
            else if (givenDob == DateTime.MinValue || givenDob == default(DateTime))
            {
                givenDob = DateTime.Now.AddYears(-18).Date + new TimeSpan(8, 0, 0);
            }
            return givenDob;
        }
        /// <summary>
        /// Attempt to fetch string value from a nullable value. Recommended for parsing DB values.
        /// </summary>
        /// <param name="input">Any object input.</param>
        /// <returns>String value. Empty string is returned if the input is null.</returns>
        public static string TryGetToString(object input)
        {
            if (input == null || Convert.IsDBNull(input))
            {
                return string.Empty;
            }
            else
            {
                return input.ToString().Trim();
            }
        }
        /// <summary>
        /// Attempt to fetch integer value from a nullable value. Recommended for parsing DB values.
        /// </summary>
        /// <param name="input">Any object input that is expected to hold an integer value.</param>
        /// <returns>int value. 0 is returned if the input is null or not a number.</returns>
        public static int TryGetIntValue(object input)
        {
            if (input == null || Convert.IsDBNull(input))
            {
                return 0;
            }
            else
            {
                string strVal = input.ToString().Trim();
                if (int.TryParse(strVal, out int numResult))
                {
                    return numResult;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Attempt to fetch decimal value from a nullable value. Recommended for parsing DB values.
        /// </summary>
        /// <param name="input">Any object input that is expected to hold a decimal or double value.</param>
        /// <returns>decimal value. decimal.Zero is returned if the input is null or not a number.</returns>
        public static decimal TryGetDecimalValue(object input)
        {
            if (input == null || Convert.IsDBNull(input))
            {
                return decimal.Zero;
            }
            else
            {
                string strVal = input.ToString().Trim();
                if (decimal.TryParse(strVal, out decimal numResult))
                {
                    return numResult;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }
        /// <summary>
        /// Attempt to fetch boolean value from a nullable value. Recommended for parsing DB values.
        /// </summary>
        /// <param name="input">Any object input.</param>
        /// <returns>boolean value. 'false' bool is returned if the input is null.</returns>
        public static bool TryGetBoolValue(object input)
        {
            if (input == null || Convert.IsDBNull(input))
            {
                return false;
            }
            else
            {
                string strVal = input.ToString().Trim();
                if (strVal == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Attempt to fetch DataTime value from a nullable value. Recommended for parsing DB values.
        /// </summary>
        /// <param name="input">Any object input.</param>
        /// <returns>DateTime value. DateTime.MinValue is returned if the input is null or unabled to be parsed.</returns>
        public static DateTime TryGetDateTimeValue(object input)
        {
            if (input == null || Convert.IsDBNull(input))
            {
                return DateTime.MinValue;
            }
            else
            {
                string strVal = input.ToString().Trim();
                if (DateTime.TryParse(strVal, out DateTime parsedVal))
                {
                    return parsedVal;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        /// <summary>
        /// Get rounded amount of a given total price.
        /// </summary>
        /// <param name="givenMoney">Total price or nett price before rounding.</param>
        /// <returns>Nett price after rounding, up to 2 decimal points.</returns>
        public static decimal RoundCurrencyAmount(decimal givenMoney)
        {
            decimal result = Math.Round(givenMoney / 0.05m) * 0.05m;
            result = decimal.Parse(result.ToString("F2"));
            return result;
        }
        /// <summary>
        /// Validate the phone number format given to ensure it is a valid phone number.
        /// </summary>
        /// <param name="phoneNum">Phone number value.</param>
        /// <param name="errorRemarks">Error remarks to be output.</param>
        /// <param name="isSg">Is this a Singaporean phone number?</param>
        /// <returns>true if valid. false if not valid.</returns>
        public static bool ValidatePhoneNumberFormat(string phoneNum, out string errorRemarks, bool isSg = false)
        {
            bool status = false;
            int stdLgthMy = 11; //Without '6' at the front, it is 10 digits long.
            int stdLgthLongerMy = 12; //Without '6' at the front, it is 11 digits long.
            System.Text.RegularExpressions.Regex alphaRegex = new System.Text.RegularExpressions.Regex("/[a-zA-Z]/g");
            errorRemarks = "";
            if (!string.IsNullOrEmpty(phoneNum))
            {
                if (phoneNum.Contains(",") || phoneNum.Contains("-") || phoneNum.Contains(":") || phoneNum.Contains(".") || phoneNum.Contains(" "))
                {
                    errorRemarks = "Phone number contains invalid characters!";
                }
                if (alphaRegex.IsMatch(phoneNum))
                {
                    errorRemarks = "Phone number contains alphabet characters!";
                }
                if (phoneNum.Contains("+"))
                {
                    phoneNum = phoneNum.Replace("+", "");
                }
                phoneNum = phoneNum.Trim();
                if (isSg == false)
                {
                    if (phoneNum.StartsWith("601"))
                    {
                        #region Real check

                        if (phoneNum.Length >= stdLgthMy)
                        {
                            if (phoneNum.StartsWith("6011") || phoneNum.StartsWith("6015"))
                            {
                                if (phoneNum.Length == stdLgthLongerMy)
                                {
                                    status = true;
                                }
                                else
                                {
                                    errorRemarks = "Phone number too short to be valid!";
                                }
                            }
                            else if (phoneNum.Length == stdLgthMy)
                            {
                                status = true;
                            }
                            else
                            {
                                errorRemarks = "Phone number too long to be valid!";
                            }
                        }
                        else if (phoneNum.Length < stdLgthMy)
                        {
                            errorRemarks = "Phone number too short to be valid!";
                        }
                        else
                        {
                            errorRemarks = "Unable to parse phone number!";
                        }

                        #endregion
                    }
                    else
                    {
                        errorRemarks = "Not a valid mobile phone number!";
                    }
                }
                else
                {
                    //Singaporean number check goes here.
                }
            }
            else
            {
                errorRemarks = "Phone number is empty!";
            }
            return status;
        }

        /// <summary>
        /// Clean out HTML tags and syntax out from the string value.
        /// </summary>
        /// <param name="input">String input.</param>
        /// <returns>Cleaned string input with no HTML tags.</returns>
        public static string CleanOutHtmlTags(string input)
        {
            const string _htmlTagPatterns = "<.*?>";
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Replace("<p>", string.Empty).Replace("</p>", "\n");
                input = System.Text.RegularExpressions.Regex.Replace(input, _htmlTagPatterns, string.Empty);
            }
            return input;
        }

        /// <summary>
        /// Restore HTML encoded values back to its original character.
        /// </summary>
        /// <param name="input">String input.</param>
        /// <returns>HTML-decoded string input.</returns>
        public static string DecodeSymbolsFromText(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = WebUtility.HtmlDecode(input);
                //If the emoji unicode failed to display properly, remove it.
                input = input.Replace("??", "");
            }
            return input;
        }

        /// <summary>
        /// Restore HTML encoded values and clean out any HTML syntaxes and tags.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DecodeSymbolsAndCleanOutHtmlTags(string input)
        {
            return DecodeSymbolsFromText(CleanOutHtmlTags(input));
        }

        #endregion

        #region Distance Between Two Coordinates

        /// <summary>
        /// Calcaulate the straight line distance between 2 points.
        /// </summary>
        /// <param name="longitude">1st coordinate's longitude</param>
        /// <param name="latitude">1st coordinate's latitude</param>
        /// <param name="otherLongitude">2nd coordinate's longitude</param>
        /// <param name="otherLatitude">2nd coordinate's latitude</param>
        /// <returns>Distance between 2 points in meters.</returns>
        public static double GetDistanceBetweeenTwoPoints(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            //Reference: https://stackoverflow.com/a/51839058
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))); //Return in meters
        }

        #endregion

        #region Get Settings Value

        /// <summary>
        /// Generate a Key-Value dictionary based on a whole string.
        /// </summary>
        /// <param name="rawStr">Setting or property values saved in string form, with each prop separated by a semi-colon and the key-value is separated by an equal sign</param>
        /// <returns>Dictionary object consists of settings in key-value format.</returns>
        public static Dictionary<string, string> GetSettingListValue(string rawStr)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(rawStr))
            {
                //Split the setting values separated by ';' (semi-colon).
                string[] splitSettings = rawStr.Split(';');
                foreach (string splitSetting in splitSettings)
                {
                    //Run through each setting values...
                    //And then split again as property strings separated by '=' (equal).
                    string[] splitProp = splitSetting.Split('=');
                    //It is a value prop value if the split length is equal to 2.
                    if (splitProp.Length == 2)
                    {
                        resp.Add(splitProp[0], splitProp[1]);
                    }
                }
            }
            return resp;
        }

        /// <summary>
        /// Generate a Key-Value dictionary based on a whole string.
        /// </summary>
        /// <param name="rawObj">Setting or property values saved in string form, with each prop separated by a semi-colon and the key-value is separated by an equal sign</param>
        /// <returns>Dictionary object consists of settings in key-value format.</returns>
        public static Dictionary<string, string> GetSettingListValue(object rawObj)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            string rawStr = TryGetToString(rawObj);
            if (!string.IsNullOrWhiteSpace(rawStr))
            {
                //Split the setting values separated by ';' (semi-colon).
                string[] splitSettings = rawStr.Split(';');
                foreach (string splitSetting in splitSettings)
                {
                    //Run through each setting values...
                    //And then split again as property strings separated by '=' (equal).
                    string[] splitProp = splitSetting.Split('=');
                    //It is a value prop value if the split length is equal to 2.
                    if (splitProp.Length == 2)
                    {
                        resp.Add(splitProp[0], splitProp[1]);
                    }
                }
            }
            return resp;
        }

        #endregion

        #region Validate Customer Order

        //private static readonly List<Order.OrderMenuList> _MenuIdListToValidate = new List<Order.OrderMenuList>()
        //{
        //    new Order.OrderMenuList() { itemId = "140048", itemCategoryId = "LBCat14" }
        //};

        ///// <summary>
        ///// Check and correct any missing modifiers of a combo menu in an order request.
        ///// </summary>
        ///// <param name="request">Order.OrderRequest object. Customer's order request.</param>
        ///// <returns>Order.OrderRequest object with possible corrections on Order.OrderRequest.orderMenuList</returns>
        //public static Order.OrderRequest ValidateOrderRequest(Order.OrderRequest request)
        //{
        //    //Ensure that the order request is not null!
        //    if (request != null)
        //    {
        //        //Proceed only if the order is not empty or is populated.
        //        if (request.orderMenuList.Count > 0)
        //        {
        //            foreach (var order in request.orderMenuList)
        //            {
        //                //Proceed to the next step if one of the order is the one we want to validate.
        //                if (_MenuIdListToValidate.Any(listMenu => listMenu.itemId == order.itemId && listMenu.itemCategoryId == order.itemCategoryId) == true)
        //                {
        //                    //Ensure that the property is not null first!
        //                    if (order.orderModifiertypes == null)
        //                    {
        //                        order.orderModifiertypes = new List<Order.OrderModifiertypes>();
        //                    }
        //                    //If this order has no modifiers, proceed to add in the modifiers.
        //                    if (order.orderModifiertypes.Count == 0)
        //                    {
        //                        switch (order.itemId)
        //                        {
        //                            case "140048": //For Free RB (R) & Coney Chicken
        //                                {
        //                                    //Create the RB (R) modifier
        //                                    Order.OrderModifiertypes drinkModGroup = new Order.OrderModifiertypes()
        //                                    {
        //                                        modifierId = "MODIFIERGROUP - 1",
        //                                        name = "RB (R)",
        //                                        qty = 1,
        //                                        price = 0.00m,
        //                                        orderModifiers = new List<Order.OrderModifiers>()
        //                                    };
        //                                    Order.OrderModifiers drinkModItem = new Order.OrderModifiers()
        //                                    {
        //                                        modifierId = "150047",
        //                                        name = "RB (R)",
        //                                        qty = 1,
        //                                        price = 0.00m,
        //                                        priceWOTax = 0.00m,
        //                                        tax = 0.00m
        //                                    };
        //                                    drinkModGroup.orderModifiers.Add(drinkModItem);
        //                                    //Create the Chicken Coney modifier
        //                                    Order.OrderModifiertypes mainModGroup = new Order.OrderModifiertypes()
        //                                    {
        //                                        modifierId = "MODIFIERGROUP - 2",
        //                                        name = "Chicken Coney",
        //                                        qty = 1,
        //                                        price = 0.00m,
        //                                        orderModifiers = new List<Order.OrderModifiers>()
        //                                    };
        //                                    Order.OrderModifiers mainModItem = new Order.OrderModifiers()
        //                                    {
        //                                        modifierId = "150021",
        //                                        name = "Chicken Coney",
        //                                        qty = 1,
        //                                        price = 0.00m,
        //                                        priceWOTax = 0.00m,
        //                                        tax = 0.00m
        //                                    };
        //                                    mainModGroup.orderModifiers.Add(mainModItem);
        //                                    //Once finish setting up the modifiers, add it to the menu's modifier list.
        //                                    order.orderModifiertypes.Add(drinkModGroup);
        //                                    order.orderModifiertypes.Add(mainModGroup);
        //                                }
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return request;
        //}

        #endregion

        #region Generator

        /// <summary>
        /// Generate serial code based on the integer input.
        /// </summary>
        /// <param name="inputId"></param>
        /// <param name="cDt"></param>
        /// <returns></returns>
        public static string GenerateNewSerialCode(int inputId, DateTime? cDt = null)
        {
            string output = "";
            if (cDt == null)
                cDt = DateTime.Now;
            output += cDt?.ToString("yyyy-MM-dd-");
            output += inputId.ToString("D5");
            output += cDt?.ToString("-ss");
            output += cDt?.ToString("mm");
            output += cDt?.ToString("HH");
            return output;
        }

        #endregion
    }
}    