//
// ColorTable.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2013-2022 Magic-Sessions. (www.magic-sessions.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;

namespace MsgReader.Rtf
{
	/// <summary>
	/// Binary data buffer
	/// </summary>
	internal class ByteBuffer
    {
        #region Fields
        /// <summary>
		/// Current contains validate bytes
		/// </summary>
		protected int IntCount;

        /// <summary>
        /// byte array 
        /// </summary>
        protected byte[] BsBuffer = new byte[16];
        #endregion

        #region Clear
        /// <summary>
		/// Clear object's data
		/// </summary>
		public virtual void Clear()
		{
			BsBuffer = new byte[ 16 ];
			IntCount = 0 ;
		}
        #endregion

        #region Reset
        /// <summary>
		/// Reset current position without clear buffer
		/// </summary>
		public void Reset()
		{
			IntCount = 0 ;
		}
        #endregion

        #region This
        /// <summary>
		/// Set of get byte at special index which starts with 0
		/// </summary>
		public byte this[ int index ]
		{
            get { return BsBuffer[index]; }
            set { BsBuffer[index] = value; }
		}
        #endregion

        #region Count
        /// <summary>
		/// Validate bytes count
		/// </summary>
		public virtual int Count => IntCount;
        #endregion

        #region Add
        /// <summary>
		/// Add a byte
		/// </summary>
		/// <param name="b">byte data</param>
		public void Add( byte b )
		{
			FixBuffer( IntCount + 1 );
			BsBuffer[IntCount] = b ;
			IntCount ++;
		}

		/// <summary>
		/// Add bytes
		/// </summary>
		/// <param name="bs">bytes</param>
		public void Add( byte[] bs )
		{
			if( bs != null)
				Add( bs , 0 , bs.Length );
		}

		/// <summary>
		/// Add bytes
		/// </summary>
		/// <param name="bs">Bytes</param>
		/// <param name="startIndex">Start index</param>
		/// <param name="length">Length</param>
		public void Add(byte[] bs , int startIndex , int length )
		{
			if( bs != null && startIndex >= 0 && (startIndex + length ) <= bs.Length && length > 0 )
			{
				FixBuffer(IntCount + length );
				Array.Copy(bs , startIndex , BsBuffer , IntCount , length );
				IntCount += length ;
			}
		}
        #endregion

        #region ToArray
        /// <summary>
		/// Get validate bytes array
		/// </summary>
		/// <returns>bytes array</returns>
		public byte[] ToArray()
		{
		    if( IntCount > 0 )
			{
				var bs = new byte[ IntCount ];
				Array.Copy( BsBuffer , 0 , bs , 0 , IntCount );
				return bs ;
			}
		    return null;
		}
        #endregion

        #region GetString
        /// <summary>
		/// Convert bytes data to a string
		/// </summary>
		/// <param name="encoding">string encoding</param>
		/// <returns>string data</returns>
		public string GetString( System.Text.Encoding encoding )
	    {
	        if( encoding == null )
				throw new ArgumentNullException(nameof(encoding));
	        return IntCount > 0 ? encoding.GetString( BsBuffer , 0 , IntCount ) : string.Empty;
	    }
        #endregion

        #region FixBuffer
        /// <summary>
		/// Fix inner buffer so it can fit to new size of buffer
		/// </summary>
		/// <param name="newSize">new size</param>
		protected void FixBuffer( int newSize )
		{
			if( newSize <= BsBuffer.Length )
				return ;

			if( newSize < (int)( BsBuffer.Length * 1.5 ))
				newSize = (int)( BsBuffer.Length * 1.5 );
			
			var bs = new byte[ newSize ];
			Buffer.BlockCopy( BsBuffer , 0 , bs , 0 , BsBuffer.Length );
			//Array.Copy( bsBuffer , 0 , bs , 0 , bsBuffer.Length );
			BsBuffer = bs ;
        }
        #endregion
    }
}