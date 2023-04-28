using Novell.Directory.Ldap;
using System;
using Terminal.DbModels;

namespace Terminal.Functions
{
    class LDapFunctions
    {
        public static bool ldapLogin(ref UserHistory user)
        {
            try
            {
                bool logged = false;

                if (!String.IsNullOrWhiteSpace(App.Settings.ldapServerIp))
                {
                    using (var cn = new LdapConnection() { ConnectionTimeout = 10000, SecureSocketLayer = false })
                    {
                        cn.Connect(App.Settings.ldapServerIp, (int)App.Settings.ldapPort);

                        if (cn.Connected)
                        {
                            cn.Bind(LdapConnection.Ldap_V3, "uid=" + user.username + "," + App.Settings.ldapDN, user.password);
                            logged = cn.Bound;

                            if (logged) { user.loginTime = DateTime.Now; }

                            LdapSearchResults searchResults = cn.Search(
                                App.Settings.roleDN,
                                LdapConnection.Ldap_V3,
                                "(objectclass=*)",
                                null, // no specified attributes
                                false // return attr and value
                                      //,new LdapSearchConstraints{TimeLimit = 15000}
                                );

                            while (searchResults.HasMore())
                            {
                                var nextEntry = searchResults.Next();
                                nextEntry.getAttributeSet();
                                user.role = nextEntry.getAttribute("cn").StringValue;
                                user.wbs = nextEntry.getAttribute("omtpwbs").StringValue;
                                user.location = user.role.Split('_')[2];
                                user.costCenter = nextEntry.getAttribute("omtcostcenter").StringValue;
                                user.WorkCenter = nextEntry.getAttribute("omtworkcenter").StringValue;
                                user.cid = nextEntry.getAttribute("omtzqpartner").StringValue;
                            }

                            searchResults = cn.Search(
                                "uid=" + user.username + "," + App.Settings.ldapDN,
                                LdapConnection.SCOPE_BASE,
                                "(objectclass=*)",
                                null,
                                false
                            );

                            while (searchResults.HasMore())
                            {
                                var nextEntry = searchResults.Next();
                                nextEntry.getAttributeSet();
                                user.ldapName = nextEntry.getAttribute("givenName").StringValue;
                                user.ldapSurname = nextEntry.getAttribute("sn").StringValue;
                                user.mail = nextEntry.getAttribute("mail").StringValue;
                                user.mobile = nextEntry.getAttribute("mobile").StringValue;
                            }
                        }
                    }
                }
                return logged;
            }
            catch (Exception e) { return false; }
        }
    }
}
