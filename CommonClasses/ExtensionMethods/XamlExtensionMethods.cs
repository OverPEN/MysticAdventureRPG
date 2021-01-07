using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static WeaponDamageType GetXmlAttributeAsDamageType(this XmlNode node, string attributeName)
        {
            string attribute = GetXmlAttributeAsString(node, attributeName);

            switch (attribute)
            {
                case "Taglio":
                    return WeaponDamageType.Taglio;
                case "Magico":
                    return WeaponDamageType.Magico;
                case "Schianto":
                    return WeaponDamageType.Schianto;
                default:
                    return WeaponDamageType.Penetrante;
            }

        }
    }
}
