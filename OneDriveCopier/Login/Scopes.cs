using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier.Login
{
    public static class Tenants
    {
        public const string Common = "common";
        public const string Organizations = "organizations";
        public const string Consumers = "consumers";
    }

    //
    // Summary:
    //     Graph permission scopes.
    public static class Scopes
    {
        //
        // Summary:
        //     Read user calendars.
        public const string CalendarsRead = "Calendars.Read";
        //
        // Summary:
        //     Read user and shared mail.
        public const string MailReadShared = "Mail.Read.Shared";
        //
        // Summary:
        //     Read and write user and shared mail.
        public const string MailReadWriteShared = "Mail.ReadWrite.Shared";
        //
        // Summary:
        //     Send mail as a user.
        public const string MailSend = "Mail.Send";
        //
        // Summary:
        //     Send mail on behalf of others.
        public const string MailSendShared = "Mail.Send.Shared";
        //
        // Summary:
        //     Read user mailbox settings.
        public const string MailboxSettingsRead = "MailboxSettings.Read";
        //
        // Summary:
        //     Read and write user mailbox settings.
        public const string MailboxSettingsReadWrite = "MailboxSettings.ReadWrite";
        //
        // Summary:
        //     Read hidden memberships.
        public const string MemberReadHidden = "Member.Read.Hidden";
        //
        // Summary:
        //     Read user OneNote notebooks.
        public const string NotesRead = "Notes.Read";
        //
        // Summary:
        //     Create user OneNote notebooks.
        public const string NotesCreate = "Notes.Create";
        //
        // Summary:
        //     Read and write user OneNote notebooks.
        public const string NotesReadWrite = "Notes.ReadWrite";
        //
        // Summary:
        //     Read all OneNote notebooks that user can access.
        public const string NotesReadAll = "Notes.Read.All";
        //
        // Summary:
        //     Read and write all OneNote notebooks that user can access.
        public const string NotesReadWriteAll = "Notes.ReadWrite.All";
        //
        // Summary:
        //     Limited notebook access (deprecated).
        public const string NotesReadWriteCreatedByApp = "Notes.ReadWrite.CreatedByApp";
        //
        // Summary:
        //     View users' email address.
        public const string Email = "email";
        //
        // Summary:
        //     Access user's data anytime.
        public const string OfflineAccess = "offline_access";
        //
        // Summary:
        //     Sign users in.
        public const string OpenId = "openid";
        //
        // Summary:
        //     View users' basic profile.
        public const string Profile = "profile";
        //
        // Summary:
        //     Read all users' full profiles.
        public const string UserReadAll = "User.Read.All";
        //
        // Summary:
        //     Read all users' basic profiles.
        public const string UserReadBasicAll = "User.ReadBasic.All";
        //
        // Summary:
        //     Read and write access to user profile.
        public const string UserReadWrite = "User.ReadWrite";
        //
        // Summary:
        //     Sign-in and read user profile.
        public const string UserRead = "User.Read";
        //
        // Summary:
        //     Read and write user and shared tasks.
        public const string TasksReadWriteShared = "Tasks.ReadWrite.Shared";
        //
        // Summary:
        //     Create, read, update and delete user tasks and containers.
        public const string TasksReadWrite = "Tasks.ReadWrite";
        //
        // Summary:
        //     Read and write access to user mail.
        public const string MailReadWrite = "Mail.ReadWrite";
        //
        // Summary:
        //     Read user and shared tasks.
        public const string TasksReadShared = "Tasks.Read.Shared";
        //
        // Summary:
        //     Have full control of all site collections.
        public const string SitesFullControlAll = "Sites.FullControl.All";
        //
        // Summary:
        //     Create, edit, and delete items and lists in all site collections.
        public const string SitesManageAll = "Sites.Manage.All";
        //
        // Summary:
        //     Read and write items in all site collections.
        public const string SitesReadWriteAll = "Sites.ReadWrite.All";
        //
        // Summary:
        //     Read items in all site collections.
        public const string SitesReadAll = "Sites.Read.All";
        //
        // Summary:
        //     Read all users' relevant people lists.
        public const string PeopleReadAll = "People.Read.All";
        //
        // Summary:
        //     Read users' relevant people lists.
        public const string PeopleRead = "People.Read";
        //
        // Summary:
        //     Read user tasks.
        public const string TasksRead = "Tasks.Read";
        //
        // Summary:
        //     Read user mail.
        public const string MailRead = "Mail.Read";
        //
        // Summary:
        //     Read and write identity provider information.
        public const string IdentityProviderReadWriteAll = "IdentityProvider.ReadWrite.All";
        //
        // Summary:
        //     Read identity provider information.
        public const string IdentityProviderReadAll = "IdentityProvider.Read.All";
        //
        // Summary:
        //     Read Microsoft Intune devices.
        public const string DeviceManagementManagedDevicesReadAll = "DeviceManagementManagedDevices.Read.All";
        //
        // Summary:
        //     Perform user-impacting remote actions on Microsoft Intune devices.
        public const string DeviceManagementManagedDevicesPrivilegedOperationsAll = "DeviceManagementManagedDevices.PrivilegedOperations.All";
        //
        // Summary:
        //     Read and write Microsoft Intune device configuration and policies.
        public const string DeviceManagementConfigurationReadWriteAll = "DeviceManagementConfiguration.ReadWrite.All";
        //
        // Summary:
        //     Read Microsoft Intune device configuration and policies.
        public const string DeviceManagementConfigurationReadAll = "DeviceManagementConfiguration.Read.All";
        //
        // Summary:
        //     Read and write Microsoft Intune apps.
        public const string DeviceManagementAppsReadWriteAll = "DeviceManagementApps.ReadWrite.All";
        //
        // Summary:
        //     Read Microsoft Intune apps.
        public const string DeviceManagementAppsReadAll = "DeviceManagementApps.Read.All";
        //
        // Summary:
        //     Read and write Microsoft Intune devices.
        public const string DeviceManagementManagedDevicesReadWriteAll = "DeviceManagementManagedDevices.ReadWrite.All";
        //
        // Summary:
        //     Read user devices.
        public const string DeviceRead = "Device.Read";
        //
        // Summary:
        //     Have full access to user contacts.
        public const string ContactsReadWrite = "Contacts.ReadWrite";
        //
        // Summary:
        //     Read user and shared contacts.
        public const string ContactsReadShared = "Contacts.Read.Shared";
        //
        // Summary:
        //     Read user contacts.
        public const string ContactsRead = "Contacts.Read";
        //
        // Summary:
        //     Read and write user and shared calendars.
        public const string CalendarsReadWriteShared = "Calendars.ReadWrite.Shared";
        //
        // Summary:
        //     Have full access to user calendars.
        public const string CalendarsReadWrite = "Calendars.ReadWrite";
        //
        // Summary:
        //     Read user and shared calendars.
        public const string CalendarsReadShare = "Calendars.Read.Shared";
        //
        // Summary:
        //     Read and write user and shared contacts.
        public const string ContactsReadWriteShared = "Contacts.ReadWrite.Shared";
        //
        // Summary:
        //     Read and write all users' full profiles.
        public const string UserReadWriteAll = "User.ReadWrite.All";
        //
        // Summary:
        //     Read Microsoft Intune RBAC settings.
        public const string DeviceManagementRBACReadAll = "DeviceManagementRBAC.Read.All";
        //
        // Summary:
        //     Read Microsoft Intune configuration.
        public const string DeviceManagementServiceConfigReadAll = "DeviceManagementServiceConfig.Read.All";
        //
        // Summary:
        //     Read identity risk event information.
        public const string IdentityRiskEventReadAll = "IdentityRiskEvent.Read.All";
        //
        // Summary:
        //     Read and write all groups.
        public const string GroupReadWriteAll = "Group.ReadWrite.All";
        //
        // Summary:
        //     Read all groups.
        public const string GroupReadAll = "Group.Read.All";
        //
        // Summary:
        //     Read and write files that the user selects.
        public const string FilesReadWriteSelected = "Files.ReadWrite.Selected";
        //
        // Summary:
        //     Read files that the user selects.
        public const string FilesReadSelected = "Files.Read.Selected";
        //
        // Summary:
        //     Have full access to the application's folder (preview).
        public const string FilesReadWriteAppFolder = "Files.ReadWrite.AppFolder";
        //
        // Summary:
        //     Read and write Microsoft Intune RBAC settings.
        public const string DeviceManagementRBACReadWriteAll = "DeviceManagementRBAC.ReadWrite.All";
        //
        // Summary:
        //     Have full access to all files user can access.
        public const string FilesReadWriteAll = "Files.ReadWrite.All";
        //
        // Summary:
        //     Read all files that user can access.
        public const string FilesReadAll = "Files.Read.All";
        //
        // Summary:
        //     Read user files.
        public const string FilesRead = "Files.Read";
        //
        // Summary:
        //     Access directory as the signed-in user.
        public const string DirectoryAccessAsUserAll = "Directory.AccessAsUser.All";
        //
        // Summary:
        //     Read and write directory data.
        public const string DirectoryReadWriteAll = "Directory.ReadWrite.All";
        //
        // Summary:
        //     Read directory data.
        public const string DirectoryReadAll = "Directory.Read.All";
        //
        // Summary:
        //     Read and write Microsoft Intune configuration.
        public const string DeviceManagementServiceConfigReadWriteAll = "DeviceManagementServiceConfig.ReadWrite.All";
        //
        // Summary:
        //     Have full access to user files.
        public const string FilesReadWrite = "Files.ReadWrite";
        //
        // Summary:
        //     Invite guest users to the organization.
        public const string UserInviteAll = "User.Invite.All";
    }
}
