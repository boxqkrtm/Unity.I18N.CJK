using System;
using System.Text;

namespace I18N.CJK {
    [Serializable]
    public class CP949 : DbcsEncoding {
        private const int UHC_CODE_PAGE = 949;

        public CP949()
          : base(949) {
        }

        public override string BodyName => "ks_c_5601-1987";

        public override string EncodingName => "Korean (UHC)";

        public override string HeaderName => "ks_c_5601-1987";

        public override string WebName => "ks_c_5601-1987";


        internal override DbcsConvert GetConvert() => DbcsConvert.KS;
        public virtual unsafe int GetBytesInternal(char[] chars, int charCount, byte[] bytes, int byteCount) {
            int num1 = 0;
            int num2 = 0;
            DbcsConvert convert = this.GetConvert();
            EncoderFallbackBuffer encoderFallbackBuffer = (EncoderFallbackBuffer)null;
            int num3 = num2;
            while (charCount-- > 0) {
                char ch = chars[num1++];
                if (ch <= '\u0080' || ch == 'ÿ') {
                    bytes[num2++] = (byte)ch;
                } else {
                    byte num4 = convert.u2n[(int)ch * 2];
                    byte num5 = convert.u2n[(int)ch * 2 + 1];
                    if (num4 == (byte)0 && num5 == (byte)0) {
                        base.HandleFallback(ref encoderFallbackBuffer, chars, ref num1, ref charCount, bytes, ref num2, ref byteCount, null);
                    } else {
                        bytes[num2++] = num4;
                        bytes[num2++] = num5;
                    }
                }
            }
            return num2 - num3;
        }
        public override int GetByteCount(char[] chars, int index, int count) {
            return GetBytes(chars, index, count, null, 0);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {
          return GetBytesInternal(chars, charCount, bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count) {
            return GetDecoder().GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {
            return GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override Decoder GetDecoder() {
            return new CP936Decoder(GetConvert());
        }
    }
}
