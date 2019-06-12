using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Enums
{
    public class EnumHelper
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Descricao;

        public string Descricao
        {
            get { return _Descricao; }
            set { _Descricao = value; }
        }


        public static EnumHelper Obter<T> (string descricao) => ObterTodos<T>(false).Where(a => a.Descricao == descricao).FirstOrDefault( );

        public static EnumHelper Obter<T> (int id) => ObterTodos<T>(false).Where(a => a.ID == id).FirstOrDefault( );

        public static IEnumerable<EnumHelper> ObterTodos<T> ( )
        {
            var valoresEnum = Enum.GetValues(typeof(T)).Cast<T>( );
            var quantidade =  valoresEnum.Count();

            foreach (var valor in valoresEnum)
                yield return (new EnumHelper( ) { Descricao = ObtemAtributo(valor), ID = Convert.ToInt32(valor) });

        }

        public static IEnumerable<EnumHelper> ObterTodos<T> (bool descricao = true)
        {
            var valoresEnum = Enum.GetValues(typeof(T)).Cast<T>( );
            var quantidade =  valoresEnum.Count();

            foreach (var valor in valoresEnum)
                yield return (new EnumHelper( ) { Descricao = valor.ToString( ), ID = Convert.ToInt32(valor) });

        }

        private static string ObtemAtributo<T> (T valor)
        {
            MemberInfo memberInfo = valor.GetType().GetMember(valor.ToString()).FirstOrDefault();

            var descriptionAttribute = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            if (descriptionAttribute != null)
                return ((DescriptionAttribute)descriptionAttribute).Description;

            return string.Empty;
        }
    }
}
