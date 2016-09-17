/* 
 * Translation by Lensual
 * https://github.com/Lensual/MinecraftServerPing-CSharp
 */

/*
 * Copyright 2014 jamietech. All rights reserved.
 * https://github.com/jamietech/MinecraftServerPing
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 *
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ''AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and contributors and should not be interpreted as representing official policies,
 * either expressed or implied, of anybody else.
 */

using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MinecraftServerPing_CSharp
{
    public class MinecraftPing
    {

        /// <summary>
        /// Fetches a MinecraftPingReply for the supplied hostname.
        /// Assumed timeout of 2s and port of 25565.
        /// </summary>
        /// <param name="hostname">a valid String hostname</param>
        /// <returns>MinecraftPingReply</returns>
        public MinecraftPingReply getPing(String hostname)
        {
            return this.getPing(new MinecraftPingOptions().setHostname(hostname));
        }

        /// <summary>
        /// Fetches a link MinecraftPingReply for the supplied options.
        /// </summary>
        /// <param name="options">a filled instance of MinecraftPingOptions</param>
        /// <returns>MinecraftPingReply</returns>
        public MinecraftPingReply getPing(MinecraftPingOptions options)
        {
            MinecraftPingUtil.validate(options.getHostname(), "Hostname cannot be null.");
            MinecraftPingUtil.validate(options.getPort(), "Port cannot be null.");

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = options.getPort();
            client.SendTimeout = options.getPort();
            client.Connect(options.getHostname(), options.getPort());

            Stream stream = client.GetStream();

            //> Handshake
            MemoryStream handshake = new MemoryStream();

            handshake.WriteByte(MinecraftPingUtil.PACKET_HANDSHAKE);
            MinecraftPingUtil.writeVarInt(handshake, MinecraftPingUtil.PROTOCOL_VERSION);
            MinecraftPingUtil.writeVarInt(handshake, options.getHostname().Length);
            handshake.Write(Encoding.UTF8.GetBytes(options.getHostname()), 0, Encoding.UTF8.GetBytes(options.getHostname()).Length);
            handshake.Write(System.BitConverter.GetBytes((short)options.getPort()), 0, 2);
            MinecraftPingUtil.writeVarInt(handshake, MinecraftPingUtil.STATUS_HANDSHAKE);

            MinecraftPingUtil.writeVarInt(stream, (int)handshake.Length);
            byte[] b = handshake.ToArray();
            stream.Write(b, 0, b.Length);

            //> Status request

            stream.WriteByte(0x01); // Size of packet
            stream.WriteByte(MinecraftPingUtil.PACKET_STATUSREQUEST);

            //< Status response

            MinecraftPingUtil.readVarInt(stream); // Size
            int id = MinecraftPingUtil.readVarInt(stream);

            MinecraftPingUtil.io(id == -1, "Server prematurely ended stream.");
            MinecraftPingUtil.io(id != MinecraftPingUtil.PACKET_STATUSREQUEST, "Server returned invalid packet.");

            int length = MinecraftPingUtil.readVarInt(stream);
            MinecraftPingUtil.io(length == -1, "Server prematurely ended stream.");
            MinecraftPingUtil.io(length == 0, "Server returned unexpected value.");

            byte[] data = new byte[length];
            stream.Read(data, 0, length);

            string json = Encoding.GetEncoding(options.getCharset()).GetString(data);

            //> Ping

            stream.WriteByte(0x09); // Size of packet
            stream.WriteByte(MinecraftPingUtil.PACKET_PING);
            stream.Write(System.BitConverter.GetBytes((long)Environment.TickCount), 0, 8); //long 8 byte

            //< Ping

            MinecraftPingUtil.readVarInt(stream); // Size
            id = MinecraftPingUtil.readVarInt(stream);
            MinecraftPingUtil.io(id == -1, "Server prematurely ended stream.");
            MinecraftPingUtil.io(id != MinecraftPingUtil.PACKET_PING, "Server returned invalid packet.");

            // Close

            handshake.Close();
            stream.Close();
            client.Close();

            Console.WriteLine(json);

            //声明定义 内存流 和 Serializer
            MemoryStream MemStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MinecraftPingReply));

            //反序列化内存流 并返回
            return (MinecraftPingReply)ser.ReadObject(MemStream);
        }
    }

}