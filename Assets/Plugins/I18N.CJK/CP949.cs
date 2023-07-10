using System;
using System.Text;

namespace I18N.CJK {
    [Serializable]
    public class CP949 : KoreanEncoding {
        private const int UHC_CODE_PAGE = 949;

        public CP949()
          : base(949, true) {
        }

        public virtual string BodyName => "ks_c_5601-1987";

        public virtual string EncodingName => "Korean (UHC)";

        public virtual string HeaderName => "ks_c_5601-1987";

        public virtual string WebName => "ks_c_5601-1987";
    }
}
