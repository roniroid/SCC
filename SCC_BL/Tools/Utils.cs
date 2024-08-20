using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Tools
{
    public static class Utils
    {
        public const char DEFAULT_DIVIDER = ';';
        public static string[] SplitAndTrim(string @string, char divider = DEFAULT_DIVIDER)
        {
            return @string
                .Split(divider)
                .ToList()
                .Select(s => s.Trim())
                .ToArray();
        }

        public static string GetMessage(object obj, Type attributeType, string fieldName)
        {
            object attr = obj.GetType().GetCustomAttributes(attributeType, true)[0];
            return attr != null ? attr.GetType().GetProperty(fieldName).GetValue(obj).ToString() : string.Empty;
        }

        public static Settings.Notification.Type GetMessageType(object obj, Type attributeType, string fieldName)
        {
            object attr = obj.GetType().GetCustomAttributes(attributeType, true)[0];
            return attr != null ? (Settings.Notification.Type)attr.GetType().GetProperty(fieldName).GetValue(obj) : Settings.Notification.Type.SUCCESS;
        }

        public static string GenerateRandomString()
        {
            int desiredLength = SCC_BL.Settings.Overall.DEFAULT_PASSWORD_LENGTH;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            return new string(Enumerable.Repeat(chars, desiredLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static T ParseStringToType<T>(string value)
        {
            dynamic res = new Object();
            try
            {
                res = (T)Activator.CreateInstance(typeof(T));
            }
            catch (Exception ex)
            {
            }
            if (typeof(T) == typeof(Int16))
            {
                try
                {
                    res = Int16.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int16));
                }
                catch (Exception ex)
                {
                    res = new Int16();
                }
            }
            else
            if (typeof(T) == typeof(Int32))
            {
                try
                {
                    res = Int32.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int32));
                }
                catch (Exception ex)
                {
                    res = new Int32();
                }
            }
            else
            if (typeof(T) == typeof(Int64))
            {
                try
                {
                    res = Int64.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int64));
                }
                catch (Exception ex)
                {
                    res = new Int64();
                }
            }
            if (typeof(T) == typeof(Int16?))
            {
                try
                {
                    res = Int16.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int16));
                }
                catch (Exception ex)
                {
                    res = new Int16?();
                }
            }
            else
            if (typeof(T) == typeof(Int32?))
            {
                try
                {
                    res = Int32.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int32));
                }
                catch (Exception ex)
                {
                    res = new Int32?();
                }
            }
            else
            if (typeof(T) == typeof(Int64?))
            {
                try
                {
                    res = Int64.Parse(value);
                    res = Convert.ChangeType(res, typeof(Int64));
                }
                catch (Exception ex)
                {
                    res = new Int64?();
                }
            }
            else
            if (typeof(T) == typeof(byte?[]))
            {
                try
                {
                    res = System.Text.Encoding.UTF8.GetBytes(value);
                    res = Convert.ChangeType(res, typeof(byte));
                }
                catch (Exception ex)
                {
                    res = new byte?[0];
                }
            }
            else
            if (typeof(T) == typeof(bool?))
            {
                try
                {
                    res = bool.Parse(value);
                    res = Convert.ChangeType(res, typeof(bool));
                }
                catch (Exception ex)
                {
                    res = new bool?();
                }
            }
            else
            if (typeof(T) == typeof(char?))
            {
                try
                {
                    res = char.Parse(value);
                    res = Convert.ChangeType(res, typeof(char));
                }
                catch (Exception ex)
                {
                    res = new char?();
                }
            }
            else
            if (typeof(T) == typeof(DateTime?))
            {
                try
                {
                    res = DateTime.Parse(value);
                    res = Convert.ChangeType(res, typeof(DateTime));
                }
                catch (Exception ex)
                {
                    res = new DateTime?();
                }
            }
            else
            if (typeof(T) == typeof(DateTime))
            {
                try
                {
                    res = DateTime.Parse(value);
                    res = Convert.ChangeType(res, typeof(DateTime));
                }
                catch (Exception ex)
                {
                    res = new DateTime();
                }
            }
            else
            if (typeof(T) == typeof(Decimal?))
            {
                try
                {
                    res = Decimal.Parse(value);
                    res = Convert.ChangeType(res, typeof(Decimal));
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (value.Contains(","))
                        {
                            res = Decimal.Parse(value.Replace(',', '.'));
                            res = Convert.ChangeType(res, typeof(Decimal));
                        }
                        else if (value.Contains("."))
                        {
                            res = Decimal.Parse(value.Replace('.', ','));
                            res = Convert.ChangeType(res, typeof(Decimal));
                        }
                    }
                    catch (Exception ex2)
                    {
                        res = new Decimal?();
                    }
                }
            }
            else
            if (typeof(T) == typeof(Double?))
            {
                try
                {
                    res = Double.Parse(value);
                    res = Convert.ChangeType(res, typeof(Double));
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (value.Contains(","))
                        {
                            res = Double.Parse(value.Replace(',', '.'));
                            res = Convert.ChangeType(res, typeof(Double));
                        }
                        else if (value.Contains("."))
                        {
                            res = Double.Parse(value.Replace('.', ','));
                            res = Convert.ChangeType(res, typeof(Double));
                        }
                    }
                    catch (Exception ex2)
                    {
                        res = new Double?();
                    }
                }
            }
            else
            if (typeof(T) == typeof(float?))
            {
                try
                {
                    res = float.Parse(value);
                    res = Convert.ChangeType(res, typeof(float));
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (value.Contains(","))
                        {
                            res = float.Parse(value.Replace(',', '.'));
                            res = Convert.ChangeType(res, typeof(float));
                        }
                        else if (value.Contains("."))
                        {
                            res = float.Parse(value.Replace('.', ','));
                            res = Convert.ChangeType(res, typeof(float));
                        }
                    }
                    catch (Exception ex2)
                    {
                        res = new float?();
                    }
                }
            }
            else
            if (typeof(T) == typeof(TimeSpan?))
            {
                try
                {
                    res = new TimeSpan(Int32.Parse(value.Split(':')[0]), Int32.Parse(value.Split(':')[1]), Int32.Parse(value.Split(':')[2]));
                    res = Convert.ChangeType(res, typeof(TimeSpan));
                }
                catch (Exception ex)
                {
                    try
                    {
                        res = new TimeSpan(Int32.Parse(value.Split(':')[0]), Int32.Parse(value.Split(':')[1]), 0);
                        res = Convert.ChangeType(res, typeof(TimeSpan));
                    }
                    catch (Exception ex2)
                    {
                        res = new TimeSpan?();
                    }
                }
            }
            else
            if (typeof(T) == typeof(string))
            {
                res = value;
                res = Convert.ChangeType(res, typeof(string));
            }
            return res;
        }
    }
}
