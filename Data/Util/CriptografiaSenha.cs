using Data.Constantes;
using System;
using System.Text;

namespace Data.Util
{
    public static class CriptografiaSenha
    {
        public static string Criptografar(string senha)
        {
            var chave = Resources.SENHA_CHAVE;
            chave = chave.Replace("-", ".");

            var chaveBase64 = Base64Encode(chave);
            var chaveBase64Reverse = Reverse(chaveBase64);
            var arrayChaveBase64Reverse = chaveBase64Reverse.ToCharArray();
            var indexChave = 0;

            var senhaBase64 = Base64Encode(senha);
            var senhaBase64Reverse = Reverse(senhaBase64);
            var arraySenhaBase64Reverse = senhaBase64Reverse.ToCharArray();
            var indexSenha = 0;

            var arrayChaveSenha = new char[senhaBase64.Length + chaveBase64.Length];

            for (int index = 0; index < arrayChaveSenha.Length; index++)
            {
                switch (index)
                {
                    case var value when index < (arrayChaveBase64Reverse.Length / 2):
                        arrayChaveSenha[index] = arrayChaveBase64Reverse[indexChave];
                        indexChave++;
                        break;

                    case var value when index >= (arrayChaveBase64Reverse.Length / 2) && index < ((arrayChaveBase64Reverse.Length / 2) + (arraySenhaBase64Reverse.Length / 2)):
                        arrayChaveSenha[index] = arraySenhaBase64Reverse[indexSenha];
                        indexSenha++;
                        break;

                    case var value when index >= (arrayChaveBase64Reverse.Length / 2) && index < (arrayChaveBase64Reverse.Length + (arraySenhaBase64Reverse.Length / 2)):
                        arrayChaveSenha[index] = arrayChaveBase64Reverse[indexChave];
                        indexChave++;
                        break;

                    case var value when index >= arrayChaveBase64Reverse.Length + (arraySenhaBase64Reverse.Length / 2):
                        arrayChaveSenha[index] = arraySenhaBase64Reverse[indexSenha];
                        indexSenha++;
                        break;

                    default:
                        break;
                }
            }

            return new string(arrayChaveSenha);
        }

        public static string Descriptografar(string senha)
        {
            var chave = Base64Encode(Resources.SENHA_CHAVE);

            var primeiraParteChave = senha.Substring(0, (chave.Length / 2));

            var primeiraParteSenha = senha.Substring(primeiraParteChave.Length, ((senha.Length - chave.Length) / 2));

            var segundaParteChave = senha.Substring(primeiraParteChave.Length + primeiraParteSenha.Length, primeiraParteChave.Length);

            var segundaParteSenha = senha.Substring(chave.Length + primeiraParteSenha.Length, primeiraParteSenha.Length);

            var chaveCompleta = primeiraParteChave + segundaParteChave;

            var senhaCompleta = primeiraParteSenha + segundaParteSenha;

            var senhaCorrigida = Base64Decode(Reverse(senhaCompleta));

            return senhaCorrigida;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string plainText)
        {
            byte[] data = Convert.FromBase64String(plainText);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
