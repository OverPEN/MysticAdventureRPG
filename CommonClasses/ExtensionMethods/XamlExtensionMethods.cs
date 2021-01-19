using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CommonClasses.ExtensionMethods
{
    public static class XamlExtensionMethods
    {
        public static string GetXmlAttributeAsString(this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute == null)
            {
                throw new ArgumentException($"L'attributo '{attributeName}' non esiste");
            }

            return attribute.Value;
        }

        public static int GetXmlAttributeAsInt(this XmlNode node, string attributeName)
        {
            return Convert.ToInt32(GetXmlAttributeAsString(node, attributeName));
        }

        public static float GetXmlAttributeAsFloat(this XmlNode node, string attributeName)
        {
            return Convert.ToSingle(GetXmlAttributeAsString(node, attributeName));
        }

        public static byte GetXmlAttributeAsByte(this XmlNode node, string attributeName)
        {
            return Convert.ToByte(GetXmlAttributeAsString(node, attributeName));
        }

        public static List<byte> GetXmlAttributeAsByteList(this XmlNode node, string attributeName)
        {
            String[] stringArray = GetXmlAttributeAsString(node, attributeName).Split(',');
            List<byte> byteList = new List<byte>();
            foreach(string s in stringArray)
            {
                byteList.Add(Convert.ToByte(s));
            }
            return byteList;
        }

        public static WeaponDamageTypeEnum GetXmlAttributeAsDamageType(this XmlNode node, string attributeName)
        {
            string attribute = GetXmlAttributeAsString(node, attributeName);

            switch (attribute)
            {
                case "Taglio":
                    return WeaponDamageTypeEnum.Taglio;
                case "Magico":
                    return WeaponDamageTypeEnum.Magico;
                case "Schianto":
                    return WeaponDamageTypeEnum.Schianto;
                default:
                    return WeaponDamageTypeEnum.Penetrante;
            }

        }
    }
}
