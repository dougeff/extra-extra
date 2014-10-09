﻿using System;
using System.Collections;
using System.Xml;

namespace extra_extra
{
    internal class EmptyXmlNodeList : XmlNodeList
    {
        public override XmlNode Item(int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            return new EmptyNodeListEnumerator();
        }

        public class EmptyNodeListEnumerator : IEnumerator
        {
            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public object Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override int Count
        {
            get
            {
                return 0;
            }
        }
    }
}
