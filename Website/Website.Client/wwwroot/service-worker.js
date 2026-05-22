// In development, always fetch from the network and do not enable offline support.
// This keeps local iteration predictable while preserving the PWA shape.
self.addEventListener('fetch', () => { });