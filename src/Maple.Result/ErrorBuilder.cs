// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Helpers;
using Maple.Results.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maple.Results;

/// <summary>
///     A mutable builder of an immutable <see cref="Error" />.
/// </summary>
/// <remarks>
///     <para>
///         Use it to assemble an error step by step (e.g., adding one <see cref="ErrorDetail" /> per invalid field in a loop) and call <see cref="Build" /> to
///         obtain the <see cref="Error" />. Create it with the constructor, with one of the static methods named after the categories
///         (e.g., <see cref="Validation" />), or from an existing error with <see cref="Error.ToBuilder" />.
///     </para>
///     <para>
///         Every argument is validated when it is passed, so <see cref="Build" /> never throws.
///         Every method returns the same builder instance to allow chaining.
///     </para>
///     <para>
///         <see cref="Build" /> takes a snapshot: later changes to the builder do not affect an <see cref="Error" /> already built,
///         and the builder stays usable after <see cref="Build" />.
///     </para>
///     <para>The builder is not thread-safe.</para>
/// </remarks>
public sealed class ErrorBuilder
{
    private ErrorCategory _category;
    private ErrorUri _typeUri;
    private string _title;
    private string? _detail;
    private TemplatedMessage? _detailTemplated;
    private ErrorUri? _instanceUri;

    // Copy-on-write: the last immutable collection (shared with an Error built from or by this builder) is reused
    // until the first change, which copies it into the pending, mutable collection.
    private ReadOnlyCollection<ErrorDetail> _builtErrorDetails;
    private List<ErrorDetail>? _pendingErrorDetails;
    private ImmutableStringDictionary _builtMetadata;
    private Dictionary<string, object?>? _pendingMetadata;

    // The keys of the current metadata, compared case-insensitively, with their stored spelling: a key that differs only in case is found with a
    // lookup instead of a scan of the other keys (#234). Created once the metadata has NameValidator.CaseIndexThreshold keys, so an error with a
    // few entries allocates nothing more, and kept in step with every change of the keys until WithoutMetadata() clears them.
    private Dictionary<string, string>? _metadataKeyIndex;

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorBuilder" /> class with the mandatory members of an <see cref="Error" />.
    /// </summary>
    /// <param name="category">The category of the error.</param>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the <paramref name="category" /> is not a defined <see cref="ErrorCategory" /> value.
    /// </exception>
    /// <exception cref="ArgumentNullException">If the <paramref name="title" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="title" /> is empty or whitespace-only.</exception>
    public ErrorBuilder(ErrorCategory category, ErrorUri typeUri, string title)
    {
        ValidateCategory(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        _category = category;
        _typeUri = typeUri;
        _title = title;
        _builtErrorDetails = Error.EmptyErrorDetails;
        _builtMetadata = Error.EmptyMetadata;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorBuilder" /> class with the members of an existing <see cref="Error" />,
    ///     which were validated when it was created.
    /// </summary>
    internal ErrorBuilder(Error error)
    {
        _category = error.Category;
        _typeUri = new ErrorUri(error.TypeUri);
        _title = error.Title;
        _detail = error.Detail;
        _detailTemplated = error.DetailTemplated;
        _instanceUri = error.InstanceUri is null
            ? null
            : new ErrorUri(error.InstanceUri);

        _builtErrorDetails = error.ErrorDetailsCollection;
        _builtMetadata = error.MetadataDictionary;
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Validation" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Validation(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Validation, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unauthenticated" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unauthenticated(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unauthenticated, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unauthorized" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unauthorized(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unauthorized, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.NotFound" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder NotFound(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.NotFound, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Timeout" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Timeout(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Timeout, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Conflict" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Conflict(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Conflict, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Failure" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Failure(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Failure, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Critical" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Critical(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Critical, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.NotImplemented" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder NotImplemented(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.NotImplemented, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unavailable" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unavailable(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unavailable, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.LimitExceeded" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder LimitExceeded(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.LimitExceeded, typeUri, title);
    }

    #endregion

    #region setters

    /// <summary>
    ///     Sets the <see cref="Error.Category" />.
    /// </summary>
    /// <param name="category">The category of the error.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the <paramref name="category" /> is not a defined <see cref="ErrorCategory" /> value.
    /// </exception>
    public ErrorBuilder WithCategory(ErrorCategory category)
    {
        ValidateCategory(category);

        _category = category;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.TypeUri" />.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    public ErrorBuilder WithTypeUri(ErrorUri typeUri)
    {
        _typeUri = typeUri;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.Title" />.
    /// </summary>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="title" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="title" /> is empty or whitespace-only.</exception>
    public ErrorBuilder WithTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        _title = title;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.Detail" />: the human-readable explanation specific to this occurrence of the problem
    ///     (the <i>RFC 9457</i> “detail” member).
    /// </summary>
    /// <param name="detail">The explanation, or <see langword="null" /> to clear it.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     <para>
    ///         To add an individual error occurrence (an <see cref="ErrorDetail" />), use <see cref="AddErrorDetail(ErrorDetail)" />.
    ///     </para>
    ///     <para>
    ///         An empty or whitespace-only text is rejected: pass <see langword="null" /> when there is no detail. The text is not trimmed. When
    ///         the text comes from data (e.g., <c>exception.Message</c> in a mapper of <see cref="Result.Try(Action, Func{Exception, Error})" />),
    ///         pass <see langword="null" /> or a fallback text when it can be empty: inside a mapper, the exception replaces the original one.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentException">If the <paramref name="detail" /> is empty or whitespace-only.</exception>
    public ErrorBuilder WithDetail(string? detail)
    {
        if (detail is not null && string.IsNullOrWhiteSpace(detail))
        {
            throw new ArgumentException(
                "The detail must not be empty or whitespace-only; pass null when there is no detail.",
                nameof(detail));
        }

        _detail = detail;

        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.DetailTemplated" />.
    /// </summary>
    /// <param name="detailTemplated">The templated explanation, or <see langword="null" /> to clear it.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     <c>WithDetailTemplated(null)</c> binds to this overload and clears the value.
    /// </remarks>
    [OverloadResolutionPriority(1)]
    public ErrorBuilder WithDetailTemplated(TemplatedMessage? detailTemplated)
    {
        _detailTemplated = detailTemplated;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.DetailTemplated" /> to a new <see cref="TemplatedMessage" />.
    /// </summary>
    /// <param name="templateId">The identifier of the message template.</param>
    /// <param name="namedValues">
    ///     The parameters (names and values) that might be required to generate a localized message; values can be <see langword="null" />.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="templateId" /> or a parameter name is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="templateId" /> is empty, whitespace-only or starts or ends with white space, or a parameter name is empty,
    ///     starts or ends with white space, is repeated, or differs only in case from another parameter name.
    /// </exception>
    public ErrorBuilder WithDetailTemplated(string templateId, params ReadOnlySpan<(string Key, object? Value)> namedValues)
    {
        _detailTemplated = TemplatedMessage.Create(templateId, namedValues, nameof(templateId), nameof(namedValues));
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.InstanceUri" />.
    /// </summary>
    /// <param name="instanceUri">
    ///     A <i>URI</i> reference that identifies the specific occurrence of the problem, or <see langword="null" /> to clear it.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     <c>about:blank</c> (<see cref="ErrorUri.None" /> or a default <see cref="ErrorUri" />) has a meaning only as a type
    ///     (<i>RFC 9457</i>, section 4.2.1), so it is rejected; pass <see langword="null" /> when there is no instance.
    /// </remarks>
    /// <exception cref="ArgumentException">If the <paramref name="instanceUri" /> is <c>about:blank</c>.</exception>
    public ErrorBuilder WithInstanceUri(ErrorUri? instanceUri)
    {
        if (instanceUri == ErrorUri.None)
        {
            throw new ArgumentException(
                "The instance URI must not be ‘about:blank’, which has a meaning only as a type; pass null when there is no instance.",
                nameof(instanceUri));
        }

        _instanceUri = instanceUri;

        return this;
    }

    #endregion

    #region error details

    /// <summary>
    ///     Appends an individual error occurrence to the <see cref="Error.ErrorDetails" />.
    /// </summary>
    /// <param name="errorDetail">The error occurrence to append.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="errorDetail" /> is <see langword="null" />.</exception>
    public ErrorBuilder AddErrorDetail(ErrorDetail errorDetail)
    {
        ArgumentNullException.ThrowIfNull(errorDetail);

        GetPendingErrorDetails().Add(errorDetail);

        return this;
    }

    /// <summary>
    ///     Appends an individual error occurrence to the <see cref="Error.ErrorDetails" />.
    /// </summary>
    /// <param name="propertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
    /// <param name="detail">The human-readable explanation specific to this individual error occurrence.</param>
    /// <param name="detailTemplateId">
    ///     The identifier of the message template; when <see langword="null" />, no <see cref="ErrorDetail.DetailTemplated" /> is set.
    /// </param>
    /// <param name="detailNamedValues">
    ///     The optional parameters (names and values) that might be required to generate a localized message;
    ///     they require <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="detail" /> or a parameter name in <paramref name="detailNamedValues" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="propertyPointer" /> is not a JSON Pointer (e.g., <c>Email</c> or <c>Items[0].Name</c>; the parameter name
    ///     of the exception is <c>PropertyPointer</c>), the <paramref name="detail" /> is empty or whitespace-only (the parameter name of the
    ///     exception is <c>Detail</c>), the <paramref name="detailTemplateId" /> is empty, whitespace-only or starts or ends with white space, a
    ///     parameter name is empty, starts or ends with white space, is repeated, or differs only in case from another parameter name,
    ///     or <paramref name="detailNamedValues" /> are passed without a <paramref name="detailTemplateId" />.
    /// </exception>
    public ErrorBuilder AddErrorDetail(string? propertyPointer, string detail, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        var detailTemplated = TemplatedMessage.CreateOptional(detailTemplateId, detailNamedValues, nameof(detailTemplateId), nameof(detailNamedValues));
        var errorDetail = new ErrorDetail(propertyPointer, detail, detailTemplated);

        return AddErrorDetail(errorDetail);
    }

    #endregion

    #region metadata

    /// <summary>
    ///     Adds the specified entry to the <see cref="Error.Metadata" />, or replaces its value if the key already exists.
    /// </summary>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">The value of the metadata entry; it may be <see langword="null" />.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only, starts or ends with white space, matches (case-insensitively) a standard problem
    ///     details member: <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or <c>detailTemplated</c>, or
    ///     differs only in case from an existing key (the same key replaces the value).
    /// </exception>
    public ErrorBuilder WithMetadata(string key, object? value)
    {
        MetadataKeyValidator.Validate(key);
        ValidateCase(key, nameof(key));

        GetPendingMetadata()[key] = value;
        _metadataKeyIndex?.TryAdd(key, key);

        return this;
    }

    /// <summary>
    ///     Adds the specified entries to the <see cref="Error.Metadata" />; entries with existing keys are replaced and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">The metadata entries to add or replace; values may be <see langword="null" />.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     All keys are validated before any entry is applied, so the builder is unchanged when this method throws.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="metadata" /> or any key is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only, starts or ends with white space, or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or <c>detailTemplated</c>; or if a key differs
    ///     only in case from an existing key or from another key of the same call (the same key replaces the value).
    /// </exception>
    [OverloadResolutionPriority(1)]
    public ErrorBuilder WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var entries = metadata.ToArray();
        foreach (var pair in entries)
            MetadataKeyValidator.Validate(pair.Key);

        ReadOnlySpan<KeyValuePair<string, object?>> entriesSpan = entries;
        ValidateCase(entriesSpan, static pair => pair.Key, nameof(metadata));

        if (entries.Length == 0)
            return this;

        var pending = GetPendingMetadata();
        foreach (var pair in entries)
        {
            pending[pair.Key] = pair.Value;
            _metadataKeyIndex?.TryAdd(pair.Key, pair.Key);
        }

        return this;
    }

    /// <summary>
    ///     Adds the specified entries, given as key-value tuples, to the <see cref="Error.Metadata" />;
    ///     entries with existing keys are replaced and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">
    ///     The metadata entries to add or replace, e.g., <c>WithMetadata(("limit", 100), ("remaining", 0))</c>; values may be <see langword="null" />.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     All keys are validated before any entry is applied, so the builder is unchanged when this method throws.
    ///     A key repeated in the same call takes the last value.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If any key is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only, starts or ends with white space, or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or <c>detailTemplated</c>; or if a key differs
    ///     only in case from an existing key or from another key of the same call (the same key replaces the value).
    /// </exception>
    public ErrorBuilder WithMetadata(params ReadOnlySpan<(string Key, object? Value)> metadata)
    {
        foreach (var (key, _) in metadata)
            MetadataKeyValidator.Validate(key);

        ValidateCase(metadata, static entry => entry.Key, nameof(metadata));

        if (metadata.IsEmpty)
            return this;

        var pending = GetPendingMetadata();
        foreach (var (key, value) in metadata)
        {
            pending[key] = value;
            _metadataKeyIndex?.TryAdd(key, key);
        }

        return this;
    }

    /// <summary>
    ///     Removes the entry of the specified key from the <see cref="Error.Metadata" />.
    /// </summary>
    /// <param name="key">The key of the metadata entry to remove; an absent key is not an error.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="key" /> is empty or whitespace-only.</exception>
    public ErrorBuilder WithoutMetadata(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        IReadOnlyDictionary<string, object?> current = _pendingMetadata ?? (IReadOnlyDictionary<string, object?>)_builtMetadata;
        if (current.ContainsKey(key))
        {
            GetPendingMetadata().Remove(key);

            // The key is stored with this exact spelling, and no other key differs from it only in case.
            _metadataKeyIndex?.Remove(key);
        }

        return this;
    }

    /// <summary>
    ///     Removes all entries from the <see cref="Error.Metadata" />.
    /// </summary>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    public ErrorBuilder WithoutMetadata()
    {
        _builtMetadata = Error.EmptyMetadata;
        _pendingMetadata = null;
        _metadataKeyIndex = null;

        return this;
    }

    #endregion

    /// <summary>
    ///     Returns a new <see cref="Error" /> with the current members of the builder.
    /// </summary>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     This method never throws. The returned error is a snapshot: later changes to the builder do not affect it, and the builder can still be used.
    /// </remarks>
    public Error Build()
    {
        // The pending collections are wrapped, not copied: the builder drops its references to them right away, so the built error is the only
        // owner and nothing can change them. A later change on the builder copies the built collection again (copy-on-write).
        if (_pendingErrorDetails is not null)
        {
            _builtErrorDetails = _pendingErrorDetails.Count == 0
                ? Error.EmptyErrorDetails
                : new ReadOnlyCollection<ErrorDetail>(_pendingErrorDetails);

            _pendingErrorDetails = null;
        }

        if (_pendingMetadata is not null)
        {
            _builtMetadata = _pendingMetadata.Count == 0
                ? Error.EmptyMetadata
                : new ImmutableStringDictionary(_pendingMetadata);

            _pendingMetadata = null;
        }

        return new Error(_category, _typeUri.Value, _title, _detail, _detailTemplated, _instanceUri?.Value, _builtErrorDetails, _builtMetadata);
    }

    #region helper methods

    /// <summary>
    ///     The current metadata entries: the pending dictionary if there is one, otherwise the entries of the built one (no copy).
    /// </summary>
    private Dictionary<string, object?> CurrentMetadata => _pendingMetadata ?? _builtMetadata.Items;

    private List<ErrorDetail> GetPendingErrorDetails()
    {
        if (_pendingErrorDetails is null)
        {
            // Room for one more detail, so that adding a single detail to an existing error fills the list exactly.
            _pendingErrorDetails = new List<ErrorDetail>(_builtErrorDetails.Count + 1);
            _pendingErrorDetails.AddRange(_builtErrorDetails);
        }

        return _pendingErrorDetails;
    }

    private Dictionary<string, object?> GetPendingMetadata()
    {
        return _pendingMetadata ??= new Dictionary<string, object?>(_builtMetadata, StringComparer.Ordinal);
    }

    /// <summary>
    ///     Throws if the key differs only in case from a key of the current metadata; the same key is allowed (it replaces the value).
    /// </summary>
    private void ValidateCase(string key, string paramName)
    {
        var current = CurrentMetadata;
        if (current.ContainsKey(key))
            return;

        var existingKey = NameValidator.FindNameDifferingOnlyInCase(key, current, ref _metadataKeyIndex);
        if (existingKey is not null)
            throw MetadataKeyValidator.KeyDiffersOnlyInCase(key, existingKey, paramName);
    }

    /// <summary>
    ///     Throws if a key of the entries differs only in case from a key of the current metadata or from an earlier key of the entries;
    ///     the same key is allowed (the last value wins).
    /// </summary>
    /// <remarks>
    ///     The keys of the entries are compared with a case-insensitive index from <see cref="NameValidator.CaseIndexThreshold" /> entries on,
    ///     and with each other below it.
    /// </remarks>
    private void ValidateCase<TEntry>(ReadOnlySpan<TEntry> entries, Func<TEntry, string> keyOf, string paramName)
    {
        var earlierKeys = entries.Length >= NameValidator.CaseIndexThreshold
            ? new Dictionary<string, string>(entries.Length, StringComparer.OrdinalIgnoreCase)
            : null;

        for (var i = 0; i < entries.Length; i++)
        {
            var key = keyOf(entries[i]);
            ValidateCase(key, paramName);

            if (earlierKeys is null)
            {
                for (var j = 0; j < i; j++)
                {
                    var earlierKey = keyOf(entries[j]);
                    MetadataKeyValidator.ValidateCase(earlierKey, key, paramName);
                }
            }
            else if (!earlierKeys.TryAdd(key, key))
            {
                var earlierKey = earlierKeys[key];
                MetadataKeyValidator.ValidateCase(earlierKey, key, paramName);
            }
        }
    }

    private static void ValidateCategory(ErrorCategory category)
    {
        if (!Enum.IsDefined(category))
            throw new ArgumentOutOfRangeException(nameof(category), category, $"The error category ‘{category}’ is not defined.");
    }

    #endregion
}
