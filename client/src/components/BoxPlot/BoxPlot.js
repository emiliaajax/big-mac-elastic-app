import { Bar, BarChart, Tooltip, XAxis, YAxis } from 'recharts'

function BoxPlot ({ data, barPropertyName, xAxisPropertyName }) {
  return ( 
    <BarChart
      width={1000}
      height={600}
      data={data}
      margin={{
        top: 5,
        right: 30,
        left: 20,
        bottom: 5,
      }}
    >
      <XAxis dataKey={xAxisPropertyName} />
      <YAxis label={{ value: 'USD', position: 'insideTopLeft', dx: -20, dy: -10 }}/>
      <Tooltip cursor={false}/>
      <Bar name="Price" dataKey={barPropertyName} fill="#82ca9d"/>
    </BarChart> 
  )
}

export default BoxPlot
