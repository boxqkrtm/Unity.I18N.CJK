

using System;
using System.Text;

namespace I18N.CJK {
internal sealed class KoreanDecoder : DbcsEncoding.DbcsDecoder {
    private bool useUHC;
    private int last_byte_count;
    private int last_byte_conv;

    public KoreanDecoder(DbcsConvert convert, bool useUHC)
      : base(convert) {
        this.useUHC = useUHC;
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => this.GetCharCount(bytes, index, count, false);

    public override int GetCharCount(byte[] bytes, int index, int count, bool refresh) {
        this.CheckRange(bytes, index, count);
        int num1 = this.last_byte_count;
        this.last_byte_count = 0;
        int charCount = 0;
        while (count-- > 0) {
            int num2 = (int)bytes[index++];
            if (num1 == 0) {
                if (num2 <= 128 || num2 == (int)byte.MaxValue)
                    ++charCount;
                else
                    num1 = num2;
            } else {
                char ch;
                if (this.useUHC && num1 < 161) {
                    int num3 = 8836 + (num1 - 129) * 178;
                    int num4 = num2 < 65 || num2 > 90 ? (num2 < 97 || num2 > 122 ? (num2 < 129 || num2 > 254 ? -1 : num3 + (num2 - 129 + 52)) : num3 + (num2 - 97 + 26)) : num3 + (num2 - 65);
                    ch = num4 < 0 || num4 * 2 > this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[num4 * 2] + (uint)this.convert.n2u[num4 * 2 + 1] * 256U);
                } else if (this.useUHC && num1 <= 198 && num2 < 161) {
                    int num5 = 14532 + (num1 - 161) * 84;
                    int num6 = num2 < 65 || num2 > 90 ? (num2 < 97 || num2 > 122 ? (num2 < 129 || num2 > 160 ? -1 : num5 + (num2 - 129 + 52)) : num5 + (num2 - 97 + 26)) : num5 + (num2 - 65);
                    ch = num6 < 0 || num6 * 2 > this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[num6 * 2] + (uint)this.convert.n2u[num6 * 2 + 1] * 256U);
                } else if (num2 >= 161 && num2 <= 254) {
                    int index1 = ((num1 - 161) * 94 + num2 - 161) * 2;
                    ch = index1 < 0 || index1 >= this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[index1] + (uint)this.convert.n2u[index1 + 1] * 256U);
                } else
                    ch = char.MinValue;
                if (ch == char.MinValue)
                    ++charCount;
                else
                    ++charCount;
                num1 = 0;
            }
        }
        if (num1 != 0) {
            if (refresh) {
                ++charCount;
                this.last_byte_count = 0;
            } else
                this.last_byte_count = num1;
        }
        return charCount;
    }

    public override int GetChars(
      byte[] bytes,
      int byteIndex,
      int byteCount,
      char[] chars,
      int charIndex) {
        return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex, false);
    }

    public override int GetChars(
      byte[] bytes,
      int byteIndex,
      int byteCount,
      char[] chars,
      int charIndex,
      bool refresh) {
        this.CheckRange(bytes, byteIndex, byteCount, chars, charIndex);
        int num1 = charIndex;
        int num2 = this.last_byte_conv;
        this.last_byte_conv = 0;
        while (byteCount-- > 0) {
            int num3 = (int)bytes[byteIndex++];
            if (num2 == 0) {
                if (num3 <= 128 || num3 == (int)byte.MaxValue)
                    chars[charIndex++] = (char)num3;
                else
                    num2 = num3;
            } else {
                char ch;
                if (this.useUHC && num2 < 161) {
                    int num4 = 8836 + (num2 - 129) * 178;
                    int num5 = num3 < 65 || num3 > 90 ? (num3 < 97 || num3 > 122 ? (num3 < 129 || num3 > 254 ? -1 : num4 + (num3 - 129 + 52)) : num4 + (num3 - 97 + 26)) : num4 + (num3 - 65);
                    ch = num5 < 0 || num5 * 2 > this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[num5 * 2] + (uint)this.convert.n2u[num5 * 2 + 1] * 256U);
                } else if (this.useUHC && num2 <= 198 && num3 < 161) {
                    int num6 = 14532 + (num2 - 161) * 84;
                    int num7 = num3 < 65 || num3 > 90 ? (num3 < 97 || num3 > 122 ? (num3 < 129 || num3 > 160 ? -1 : num6 + (num3 - 129 + 52)) : num6 + (num3 - 97 + 26)) : num6 + (num3 - 65);
                    ch = num7 < 0 || num7 * 2 > this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[num7 * 2] + (uint)this.convert.n2u[num7 * 2 + 1] * 256U);
                } else if (num3 >= 161 && num3 <= 254) {
                    int index = ((num2 - 161) * 94 + num3 - 161) * 2;
                    ch = index < 0 || index >= this.convert.n2u.Length ? char.MinValue : (char)((uint)this.convert.n2u[index] + (uint)this.convert.n2u[index + 1] * 256U);
                } else
                    ch = char.MinValue;
                chars[charIndex++] = ch != char.MinValue ? ch : '?';
                num2 = 0;
            }
        }
        if (num2 != 0) {
            if (refresh) {
                chars[charIndex++] = '?';
                this.last_byte_conv = 0;
            } else
                this.last_byte_conv = num2;
        }
        return charIndex - num1;
    }
}
  }