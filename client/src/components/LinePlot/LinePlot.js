import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip } from 'recharts'

function LinePlot ({ data, xAxisProperty, yAxisProperty, lineName, yAxisName}) {
  return (
    <LineChart
      width={700}
      height={500}
      data={data}
      margin={{
        top: 5,
        right: 30,
        left: 20,
        bottom: 5,
      }}
    >
      {/* <CartesianGrid strokeDasharray="3 3" /> */}
      <XAxis
        dataKey={xAxisProperty}
        tickFormatter={(dateStr) => new Date(dateStr).toLocaleDateString()}
      />
      <YAxis label={{ value: `${yAxisName}`, position: 'insideTopLeft' }}/>
      <Tooltip labelFormatter={(dateStr) => new Date(dateStr).toLocaleDateString()}/>
      <Line
        type="monotone"
        dataKey={yAxisProperty}
        name={lineName}
        stroke="#82ca9d"
        strokeWidth={2}
      />
    </LineChart>
  )
}

export default LinePlot