using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Enums
{
    public class EnumHelper
    {
        #region Propriedades
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
#endregion
        #region Metodos Publicos
        /// <summary>
        /// Obtem a Descrição do Enum a partir do valor do ID
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="id">ID da descrição desejada</param>
        /// <returns>Retorna um Objeto com ID e Descrição do Enum</returns>
        public static EnumHelper Obter<T> (int id)
            => ObterTodos<T>( ).Where(a => a.ID == id).FirstOrDefault( ) ?? new EnumHelper( ) { Descricao = string.Empty };
       
        /// <summary>
        /// Retorna Uma Lista de ID e Descrição do conteúdo do Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <returns>Lista com ID e Descrição</returns>
        public static IEnumerable<EnumHelper> ObterTodos<T> ( )
        {
            var valoresEnum = Enum.GetValues(typeof(T)).Cast<T>( );
            var quantidade =  valoresEnum.Count();

            foreach (var valor in valoresEnum)
                yield return (new EnumHelper( ) { Descricao = ObtemAtributo(valor), ID = Convert.ToInt32(valor) });

        }

        /// <summary>
        /// Retorna uma lista com ID e Valor String de um Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param descricao="descricao">False para retornar o Valor String ao invés da descrição do Enum</param>
        /// <returns></returns>
        public static IEnumerable<EnumHelper> ObterTodos<T> (bool descricao = true)
        {
            var valoresEnum = Enum.GetValues(typeof(T)).Cast<T>( );
            var quantidade =  valoresEnum.Count();

            foreach (var valor in valoresEnum)
                yield return (new EnumHelper( ) { Descricao = valor == null ? string.Empty:valor.ToString(), ID = Convert.ToInt32(valor) });

        }
        #endregion
        #region Metodos Privados
        /// <summary>
        /// Metodo Auxiliar para Obter o Atributo descrição do Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valor"></param>
        /// <returns></returns>
        private static string ObtemAtributo<T> (T valor)
        {
            MemberInfo memberInfo = valor.GetType().GetMember(valor.ToString()).FirstOrDefault();

            var descriptionAttribute = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            if (descriptionAttribute != null)
                return ((DescriptionAttribute)descriptionAttribute).Description;

            return string.Empty;
        }
#endregion
    }
}
