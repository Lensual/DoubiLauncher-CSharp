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
using System.Collections.Generic;

namespace MinecraftServerPing_CSharp
{

    /**
     * References:
     * http://wiki.vg/Server_List_Ping
     * https://gist.github.com/thinkofdeath/6927216
     */
    public class MinecraftPingReply
    {

        public string description;
        public Players players;
        public Version version;
        public String favicon;

        /// <summary>
        /// return the MOTD
        /// </summary>
        /// <returns></returns>
        //public Description getDescription()
        //{
        //    return this.description;
        //}

        /// <summary>
        /// return Players
        /// </summary>
        /// <returns></returns>
        public Players getPlayers()
        {
            return this.players;
        }

        /// <summary>
        /// return Version
        /// </summary>
        /// <returns></returns>
        public Version getVersion()
        {
            return this.version;
        }

        /// <summary>
        /// return Base64 encoded favicon image
        /// </summary>
        /// <returns></returns>
        public String getFavicon()
        {
            return this.favicon;
        }

        public class Description
        {
            public String text;

            /// <summary>
            /// return Server description text
            /// </summary>
            /// <returns></returns>
            public String getText()
            {
                return this.text;
            }
        }

        public class Players
        {
            public int max;
            public int online;
            public List<Player> sample;

            /// <summary>
            /// return Maximum player count
            /// </summary>
            /// <returns></returns>
            public int getMax()
            {
                return this.max;
            }

            /// <summary>
            /// return Online player count
            /// </summary>
            /// <returns></returns>
            public int getOnline()
            {
                return this.online;
            }

            /// <summary>
            /// return List of some players (if any) specified by server
            /// </summary>
            /// <returns></returns>
            public List<Player> getSample()
            {
                return this.sample;
            }
        }

        public class Player
        {
            public String name;
            public String id;

            /// <summary>
            /// return Name of player
            /// </summary>
            /// <returns></returns>
            public String getName()
            {
                return this.name;
            }

            /// <summary>
            /// Unknown
            /// </summary>
            /// <returns></returns>
            public String getId()
            {
                return this.id;
            }

        }

        public class Version
        {
            public String name;
            public int protocol;

            /// <summary>
            /// return Version name (ex: 13w41a)
            /// </summary>
            /// <returns></returns>
            public String getName()
            {
                return this.name;
            }

            /// <summary>
            /// return Protocol version
            /// </summary>
            /// <returns></returns>
            public int getProtocol()
            {
                return this.protocol;
            }
        }

    }
}
