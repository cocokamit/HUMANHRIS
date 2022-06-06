<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tt.aspx.cs" Inherits="tt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="//mozilla.github.io/pdf.js/build/pdf.js"></script>
    <script type="text/javascript">

        var url = 'http://localhost:11869/MeshNetworksHRIS/style/images/arrow.png';
        console.log(url);
        PDFJS.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
        
        // Asynchronous download of PDF
       
        var loadingTask = PDFJS.getDocument(url);
        loadingTask.promise.then(function (pdf) {
            console.log('PDF loaded');

            // Fetch the first page
            var pageNumber = 1;
            pdf.getPage(pageNumber).then(function (page) {
                console.log('Page loaded');

                var scale = 1.5;
                var viewport = page.getViewport(scale);

                // Prepare canvas using PDF page dimensions
                var canvas = document.getElementById('the-canvas');
                var context = canvas.getContext('2d');
                canvas.height = viewport.height;
                canvas.width = viewport.width;

                // Render PDF page into canvas context
                var renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                var renderTask = page.render(renderContext);
                renderTask.then(function () {
                    console.log('Page rendered');
                });
            });
        }, function (reason) {
            // PDF loading error
            console.error(reason);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <canvas id="the-canvas"></canvas>
    </form>
</body>
</html>
