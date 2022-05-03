var tour = {
    autoStart: true,
    data: [
        { element: '#activitytab', tooltip: 'Click dashboard tab to see all the EPs', text: 'Click dashboard tab to see all the EPs', position: 'T' },
        { element: '#defeciencytab', tooltip: 'Click deficiency tab to see all deficient assets', text: 'Click deficiency tab to see all deficient assets', position: 'RB' },
        //{ element: '#inspectionSummarytab', tooltip: 'inspectionSummarytab', text: 'This option has 2 inspectionSummarytab', position: 'T' },
        { element: '#StandardEP', tooltip: 'Click EP to view details or inspect', text: 'Click EP to view details or inspect', position: 'B' },
        { element: '#DOC', tooltip: 'Click on images to add documents if document required', text: 'Click to add documents', position: 'LB' },
        { element: '#dash_btnfilter', tooltip: 'Quickly filter your view with these buttons', text: 'Quickly filter your view with these buttons', position: 'B' },
        //{ element: '#c_myTable', tooltip: 'this is ep records', text: 'this is ep records', position: 'T' },
        //{ element: '#Description', tooltip: 'this is ep description', text: 'this is ep description', position: 'B' }
    ],
    welcomeMessage: 'Welcome to the Dashboard ',
    controlsPosition: 'TR',
    buttons: {
        next: { text: 'Next &rarr;', class: 'btn btn-default' },
        prev: { text: '&larr; Previous', class: 'btn btn-default' },
        start: { text: 'Start', class: 'btn btn-primary' },
        end: { text: 'End', class: 'btn btn-primary' },
        all: { text: 'Help', class: 'btn btn-primary' },
        //help: { text: 'Help', class: 'btn btn-primary' }
    },
    controlsCss: {
        background: 'rgb(255, 255, 255)',
        color: '#fff',
        width: '400px',
        'border-radius': 0
    }
};

//http://alvaroveliz.github.io/aSimpleTour/