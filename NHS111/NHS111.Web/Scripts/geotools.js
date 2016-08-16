function LatLon(lat, lon, datum) {
    // allow instantiation without 'new'
    if (!(this instanceof LatLon)) return new LatLon(lat, lon, datum);

    if (datum === undefined) datum = LatLon.datum.WGS84;

    this.lat = Number(lat);
    this.lon = Number(lon);
    this.datum = datum;
}

LatLon.ellipsoid = {
    WGS84: { a: 6378137, b: 6356752.31425, f: 1 / 298.257223563 },
    GRS80: { a: 6378137, b: 6356752.31414, f: 1 / 298.257222101 },
    Airy1830: { a: 6377563.396, b: 6356256.909, f: 1 / 299.3249646 },
    AiryModified: { a: 6377340.189, b: 6356034.448, f: 1 / 299.3249646 },
    Intl1924: { a: 6378388, b: 6356911.946, f: 1 / 297 },
    Bessel1841: { a: 6377397.155, b: 6356078.963, f: 1 / 299.152815351 },
};

LatLon.datum = {
    /* eslint key-spacing: 0, comma-dangle: 0 */
    WGS84: {
        ellipsoid: LatLon.ellipsoid.WGS84,
        transform: {
            tx: 0.0, ty: 0.0, tz: 0.0,    // m
            rx: 0.0, ry: 0.0, rz: 0.0,    // sec
            s: 0.0
        }                                  // ppm
    },
    NAD83: { // (2009); functionally ≡ WGS84 - www.uvm.edu/giv/resources/WGS84_NAD83.pdf
        ellipsoid: LatLon.ellipsoid.GRS80,
        transform: {
            tx: 1.004, ty: -1.910, tz: -0.515,  // m
            rx: 0.0267, ry: 0.00034, rz: 0.011,  // sec
            s: -0.0015
        }                               // ppm
    }, // note: if you *really* need to convert WGS84<->NAD83, you need more knowledge than this!
    OSGB36: { // www.ordnancesurvey.co.uk/docs/support/guide-coordinate-systems-great-britain.pdf
        ellipsoid: LatLon.ellipsoid.Airy1830,
        transform: {
            tx: -446.448, ty: 125.157, tz: -542.060,  // m
            rx: -0.1502, ry: -0.2470, rz: -0.8421, // sec
            s: 20.4894
        }                               // ppm
    },
    ED50: { // www.gov.uk/guidance/oil-and-gas-petroleum-operations-notices#pon-4
        ellipsoid: LatLon.ellipsoid.Intl1924,
        transform: {
            tx: 89.5, ty: 93.8, tz: 123.1,    // m
            rx: 0.0, ry: 0.0, rz: 0.156,  // sec
            s: -1.2
        }                                  // ppm
    },
    Irl1975: { // osi.ie/OSI/media/OSI/Content/Publications/transformations_booklet.pdf
        ellipsoid: LatLon.ellipsoid.AiryModified,
        transform: {
            tx: -482.530, ty: 130.596, tz: -564.557,  // m
            rx: -1.042, ry: -0.214, rz: -0.631,  // sec
            s: -8.150
        }                                // ppm
    }, // note: many sources have opposite sign to rotations - to be checked!
    TokyoJapan: { // www.geocachingtoolbox.com?page=datumEllipsoidDetails
        ellipsoid: LatLon.ellipsoid.Bessel1841,
        transform: {
            tx: 148, ty: -507, tz: -685,      // m
            rx: 0, ry: 0, rz: 0,      // sec
            s: 0
        }                                    // ppm
    },
};
/** Extend Number object with method to convert numeric degrees to radians */
if (Number.prototype.toRadians === undefined) {
    Number.prototype.toRadians = function () { return this * Math.PI / 180; };
}
/** Extend Number object with method to convert radians to numeric (signed) degrees */
if (Number.prototype.toDegrees === undefined) {
    Number.prototype.toDegrees = function () { return this * 180 / Math.PI; };
}

function OsGridRef(easting, northing) {
    // allow instantiation without 'new'
    if (!(this instanceof OsGridRef)) return new OsGridRef(easting, northing);

    this.easting = Number(easting);
    this.northing = Number(northing);
}
OsGridRef.osGridToLatLon = function (gridref, datum) {
    if (!(gridref instanceof OsGridRef)) throw new TypeError('gridref is not OsGridRef object');
    if (datum === undefined) datum = LatLon.datum.WGS84;

    var E = gridref.easting;
    var N = gridref.northing;

    var a = 6377563.396, b = 6356256.909;              // Airy 1830 major & minor semi-axes
    var F0 = 0.9996012717;                             // NatGrid scale factor on central meridian
    var φ0 = (49).toRadians(), λ0 = (-2).toRadians();  // NatGrid true origin is 49°N,2°W
    var N0 = -100000, E0 = 400000;                     // northing & easting of true origin, metres
    var e2 = 1 - (b * b) / (a * a);                          // eccentricity squared
    var n = (a - b) / (a + b), n2 = n * n, n3 = n * n * n;         // n, n², n³

    var φ = φ0, M = 0;
    do {
        φ = (N - N0 - M) / (a * F0) + φ;

        var Ma = (1 + n + (5 / 4) * n2 + (5 / 4) * n3) * (φ - φ0);
        var Mb = (3 * n + 3 * n * n + (21 / 8) * n3) * Math.sin(φ - φ0) * Math.cos(φ + φ0);
        var Mc = ((15 / 8) * n2 + (15 / 8) * n3) * Math.sin(2 * (φ - φ0)) * Math.cos(2 * (φ + φ0));
        var Md = (35 / 24) * n3 * Math.sin(3 * (φ - φ0)) * Math.cos(3 * (φ + φ0));
        M = b * F0 * (Ma - Mb + Mc - Md);              // meridional arc

    } while (N - N0 - M >= 0.00001);  // ie until < 0.01mm

    var cosφ = Math.cos(φ), sinφ = Math.sin(φ);
    var ν = a * F0 / Math.sqrt(1 - e2 * sinφ * sinφ);            // nu = transverse radius of curvature
    var ρ = a * F0 * (1 - e2) / Math.pow(1 - e2 * sinφ * sinφ, 1.5); // rho = meridional radius of curvature
    var η2 = ν / ρ - 1;                                    // eta = ?

    var tanφ = Math.tan(φ);
    var tan2φ = tanφ * tanφ, tan4φ = tan2φ * tan2φ, tan6φ = tan4φ * tan2φ;
    var secφ = 1 / cosφ;
    var ν3 = ν * ν * ν, ν5 = ν3 * ν * ν, ν7 = ν5 * ν * ν;
    var VII = tanφ / (2 * ρ * ν);
    var VIII = tanφ / (24 * ρ * ν3) * (5 + 3 * tan2φ + η2 - 9 * tan2φ * η2);
    var IX = tanφ / (720 * ρ * ν5) * (61 + 90 * tan2φ + 45 * tan4φ);
    var X = secφ / ν;
    var XI = secφ / (6 * ν3) * (ν / ρ + 2 * tan2φ);
    var XII = secφ / (120 * ν5) * (5 + 28 * tan2φ + 24 * tan4φ);
    var XIIA = secφ / (5040 * ν7) * (61 + 662 * tan2φ + 1320 * tan4φ + 720 * tan6φ);

    var dE = (E - E0), dE2 = dE * dE, dE3 = dE2 * dE, dE4 = dE2 * dE2, dE5 = dE3 * dE2, dE6 = dE4 * dE2, dE7 = dE5 * dE2;
    φ = φ - VII * dE2 + VIII * dE4 - IX * dE6;
    var λ = λ0 + X * dE - XI * dE3 + XII * dE5 - XIIA * dE7;

    var point = new LatLon(φ.toDegrees(), λ.toDegrees(), LatLon.datum.OSGB36);
    if (datum != LatLon.datum.OSGB36) point = point.convertDatum(datum);

    return point;
};