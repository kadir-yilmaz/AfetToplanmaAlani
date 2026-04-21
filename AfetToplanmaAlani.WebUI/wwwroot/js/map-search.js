/**
 * Shared Map Search Service for AFAD Toplanma Alanı
 * Centralizes Nominatim API logic and address formatting.
 */
class MapSearchService {
    constructor() {
        this.baseUrl = 'https://nominatim.openstreetmap.org/search';
    }

    /**
     * Performs a search on Nominatim API
     * @param {string} query - The search text
     * @returns {Promise<Array>} - A promise that resolves to the search results
     */
    async search(query) {
        if (!query || query.length < 3) return Promise.resolve([]);

        const url = `${this.baseUrl}?format=json&q=${encodeURIComponent(query)}&limit=15&countrycodes=tr&addressdetails=1&extratags=1&namedetails=1&dedupe=1`;

        try {
            const response = await fetch(url);
            if (!response.ok) throw new Error('Network response was not ok');

            const results = await response.json();

            // Client-side deduplication based on osm_id
            const uniqueResults = [];
            const seenIds = new Set();

            for (const item of results) {
                if (!seenIds.has(item.osm_id)) {
                    seenIds.add(item.osm_id);
                    uniqueResults.push(item);
                }
            }

            return uniqueResults;
        } catch (error) {
            console.error('Map search error:', error);
            return [];
        }
    }

    /**
     * Formats the Nominatim result into a clean Title and Subtitle
     * @param {Object} item - The result item from Nominatim
     * @returns {Object} - { title: string, subtitle: string }
     */
    formatResult(item) {
        const addr = item.address || {};
        const name = item.name || addr.amenity || addr.building || addr.shop || addr.tourism || item.display_name.split(',')[0];

        // Format detailed address steps (Neighborhood -> District -> City)
        let details = [];

        // Road (if different from name)
        if (addr.road && addr.road !== name) details.push(addr.road);

        // Neighborhood / Suburb
        if (addr.suburb) details.push(addr.suburb);
        else if (addr.neighbourhood) details.push(addr.neighbourhood);
        else if (addr.neighborhood) details.push(addr.neighborhood); // Nominatim sometimes uses 'neighborhood'

        // Town / District
        if (addr.town) details.push(addr.town);
        else if (addr.district) details.push(addr.district);
        else if (addr.county) details.push(addr.county);

        // City / Province
        if (addr.city) details.push(addr.city);
        else if (addr.province) details.push(addr.province);
        else if (addr.state) details.push(addr.state);

        const subtitle = details.join(', ') || item.display_name;

        return {
            title: name,
            subtitle: subtitle
        };
    }
}

// Make it globally available
window.MapSearchService = MapSearchService;
