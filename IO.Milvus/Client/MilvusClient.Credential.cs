﻿using IO.Milvus.Grpc;
using IO.Milvus.Utils;

namespace IO.Milvus.Client;

public partial class MilvusClient
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="username">The username of the user to be created.</param>
    /// <param name="password">The password of the user to be created..</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    public async Task CreateCredentialAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(username);
        Verify.NotNullOrWhiteSpace(password);

        ulong timestamp = (ulong)TimestampUtils.GetNowUtcTimestamp();
        await InvokeAsync(_grpcClient.CreateCredentialAsync, new CreateCredentialRequest
        {
            Username = username,
            Password = Base64Encode(password),
            ModifiedUtcTimestamps = timestamp,
            CreatedUtcTimestamps = timestamp
        }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="username">The username of the user to delete.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    public async Task DeleteCredentialAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(username);

        await InvokeAsync(_grpcClient.DeleteCredentialAsync, new DeleteCredentialRequest
        {
            Username = username
        }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the password for a user.
    /// </summary>
    /// <param name="username">The username for which to change the password.</param>
    /// <param name="oldPassword">The user's old password.</param>
    /// <param name="newPassword">The new password to set for the user.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    public async Task UpdateCredentialAsync(
        string username,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(username);
        Verify.NotNullOrWhiteSpace(oldPassword);
        Verify.NotNullOrWhiteSpace(newPassword);

        await InvokeAsync(_grpcClient.UpdateCredentialAsync, new UpdateCredentialRequest
        {
            NewPassword = Base64Encode(newPassword),
            OldPassword = Base64Encode(oldPassword),
            Username = username
        }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists all users.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.
    /// </param>
    public async Task<IList<string>> ListCredUsersAsync(
        CancellationToken cancellationToken = default)
    {
        ListCredUsersResponse response = await InvokeAsync(_grpcClient.ListCredUsersAsync, new ListCredUsersRequest(), static r => r.Status, cancellationToken).ConfigureAwait(false);

        return response.Usernames;
    }
}
