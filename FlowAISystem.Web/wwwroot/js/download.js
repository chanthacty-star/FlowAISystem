function downloadFile(filename, contentType, bytes) {

    const blob = new Blob(
        [new Uint8Array(bytes)],
        {
            type: contentType
        });

    const url =
        URL.createObjectURL(blob);


    const link =
        document.createElement("a");

    link.href = url;
    link.download = filename;

    link.click();


    URL.revokeObjectURL(url);
}