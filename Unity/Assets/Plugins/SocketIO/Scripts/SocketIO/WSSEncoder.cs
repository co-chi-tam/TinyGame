#region License
/*
 * Encoder.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

//#define SOCKET_IO_DEBUG			// Uncomment this for debug
using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace SocketIO
{
	public class WSSEncoder: Encoder
	{
		public override string Encode(Packet packet)
		{
			// base.Encode (packet);
			try
			{
				#if SOCKET_IO_DEBUG
				Debug.Log("[SocketIO] Encoding: " + packet.json);
				#endif

				StringBuilder builder = new StringBuilder();
				builder.Append("{");
				// first is type
				builder.Append("\"ept\":" + (int)packet.enginePacketType + ",");
				if(!packet.enginePacketType.Equals(EnginePacketType.MESSAGE)){
					return builder.ToString();
				}

				builder.Append("\"spt\":" + (int)packet.socketPacketType + ",");

				// attachments if we have them
				if (packet.socketPacketType == SocketPacketType.BINARY_EVENT || packet.socketPacketType == SocketPacketType.BINARY_ACK) {
					builder.Append("\"atts\":" + packet.attachments + ",");
				}

				// if we have a namespace other than '/'
				// we append it followed by a comma ','
				if (!string.IsNullOrEmpty(packet.nsp) && !packet.nsp.Equals("/")) {
					builder.Append("\"nsp\":" + packet.nsp + ",");
				}

				// immediately followed by the id
				if (packet.id > -1) {
					builder.Append("\"id\":" + packet.id + ",");
				}

				builder.Append("\"mesg\":");
				if (packet.json != null && !packet.json.ToString().Equals("null")) {
					builder.Append(packet.json.ToString());
				} else {
					builder.Append("[]");
				}

				#if SOCKET_IO_DEBUG
				Debug.Log("[SocketIO] Encoded: " + builder);
				#endif

				builder.Append("}");
				return builder.ToString();

			} catch(Exception ex) {
				throw new SocketIOException("Packet encoding failed: " + packet ,ex);
			}
		}
	}
}
