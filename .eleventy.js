const eleventyNavigationPlugin = require("@11ty/eleventy-navigation");

module.exports = function (config) {

    // Configures the development server
    config.setServerOptions({
        // Default values are shown:
    
        // Whether the live reload snippet is used
        liveReload: true,
    
        // Whether DOM diffing updates are applied where possible instead of page reloads
        domDiff: true,
    
        // The starting port number
        // Will increment up to (configurable) 10 times if a port is already in use.
        port: 8080,
    
        // Additional files to watch that will trigger server updates
        // Accepts an Array of file paths or globs (passed to `chokidar.watch`).
        // Works great with a separate bundler writing files to your output folder.
        // e.g. `watch: ["_site/**/*.css"]`
        watch: [],
    
        // Show local network IP addresses for device testing
        showAllHosts: true,
    
        // Use a local key/certificate to opt-in to local HTTP/2 with https
        https: {
          // key: "./localhost.key",
          // cert: "./localhost.cert",
        },
    
        // Change the default file encoding for reading/serving files
        encoding: "utf-8",
    });


    // Collections: Übersetzungen, Sekundärliteratur, Theateraufführungen, Selbstständige und unselbstständige Drucke, Poshume Ausgaben, Forschungbibliografie
    // Reads in the Collections based on folder name and sorts them according to the rules provided by the function
    config.addCollection("uebersetzungen", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/uebersetzungen/*.html")
            .sort(function(a, b) {
                if (a.data.Sprache !== null && b.data.Sprache !== null &&
                        a.data.Sprache !== b.data.Sprache) {
                    return a.data.Sprache.localeCompare(b.data.Sprache);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("sekundaer", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/sekundaerliteratur/*.html")
            .sort(function(a, b) {
                if (a.data.Jahr !== null && b.data.Jahr !== null &&
                        a.data.Jahr !== b.data.Jahr) {
                    return a.data.Jahr - b.data.Jahr;
                } else if ( a.data.Autor !== null && b.data.Autor !== null &&
                        a.data.Autor !== b.data.Autor) {
                    return a.data.Autor.localeCompare(b.data.Autor);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("theater", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/theaterauffuehrungen/*.html")
            .sort(function(a, b) {
                if (a.data.Jahr !== null && b.data.Jahr !== null &&
                        a.data.Jahr !== b.data.Jahr) {
                    return a.data.Jahr - b.data.Jahr;
                } else if ( a.data.Name !== null && b.data.Name !== null &&
                        a.data.Name !== b.data.Name) {
                    return a.data.Name.localeCompare(b.data.Name);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("selbststaendigedrucke", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/selbststaendige_drucke/*.html")
            .sort(function(a, b) {
                if (a.data.Jahr !== null && b.data.Jahr !== null &&
                        a.data.Jahr !== b.data.Jahr) {
                    return a.data.Jahr - b.data.Jahr;
                } else if ( a.data.Autor !== null && b.data.Autor !== null &&
                        a.data.Autor !== b.data.Autor) {
                    return a.data.Autor.localeCompare(b.data.Autor);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("posthumeausgaben", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/posthume_ausgaben/*.html")
            .sort(function(a, b) {
                if (a.data.Jahr !== null && b.data.Jahr !== null &&
                        a.data.Jahr !== b.data.Jahr) {
                    return a.data.Jahr - b.data.Jahr;
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("unselbststaendigedrucke", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/unselbstaendige_drucke/*.html")
            .sort(function(a, b) {
                if ( a.data.Werk !== null && b.data.Werk !== null &&
                        a.data.Werk !== b.data.Werk) {
                    return a.data.Werk.localeCompare(b.data.Werk);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    config.addCollection("handschriften", function(collectionApi) {
        return collectionApi
            .getFilteredByGlob("**/lists/handschriften/*.html")
            .sort(function(a, b) {
                if ( a.data.Ort !== null && b.data.Ort !== null &&
                        a.data.Ort !== b.data.Ort) {
                    return a.data.Ort.localeCompare(b.data.Ort);
                } else if (a.data.Sort !== null && b.data.Sort !== null &&
                        a.data.Sort !== b.data.Sort) {
                    return a.data.Sort - b.data.Sort;
                }
                return 0;
            });
    });

    // Uses the semi-official "navigation"-plugin for eleventy 
    config.addPlugin(eleventyNavigationPlugin);
    
    // Set static folder, which copntent will be copied to the output folder
    config.addPassthroughCopy({ "src/static/": "/static/" });

    // Output directory
    var outputdir = "site";
    if (process.env.ELEVENTY_ENVIRONMENT == "production") {
        outputdir = "docs";
    }

    config.addShortcode("year", () => `${new Date().getFullYear()}`);
    config.addShortcode("month", () => `${new Date().getMonth() + 1}`);
    config.addShortcode("day", () => `${new Date().getDate()}`);

    return { 
        // Set custom directories for dynamic pages, data, includes, layouts and finally the generated output
        dir: 
            { 
                input: "src/dynamic", 
                layouts: "../layouts",
                includes: "../includes", 
                data: "../data", 
                output: outputdir 
            },
            
        // Set template formats so that other files won't be included in dist
        templateFormats: ["njk", "md", "liquid", "html"]
    };
};