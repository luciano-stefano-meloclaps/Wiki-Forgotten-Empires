export default function KintsugiDivider() {
  return (
    <div className="w-full h-1 relative overflow-hidden">
      <svg width="100%" height="4" viewBox="0 0 1200 4" className="absolute inset-0" preserveAspectRatio="none">
        <defs>
          <linearGradient id="kintsugiGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stopColor="#BFA14A" stopOpacity="0.8" />
            <stop offset="25%" stopColor="#E6D28A" stopOpacity="1" />
            <stop offset="50%" stopColor="#BFA14A" stopOpacity="0.9" />
            <stop offset="75%" stopColor="#E6D28A" stopOpacity="1" />
            <stop offset="100%" stopColor="#BFA14A" stopOpacity="0.8" />
          </linearGradient>
          <filter id="glow">
            <feGaussianBlur stdDeviation="1" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>
        <path
          d="M0,2 Q100,1 200,2 T400,2 Q500,1 600,2 T800,2 Q900,3 1000,2 T1200,2"
          stroke="url(#kintsugiGradient)"
          strokeWidth="2"
          fill="none"
          filter="url(#glow)"
          className="animate-pulse"
        />
      </svg>
    </div>
  )
}
